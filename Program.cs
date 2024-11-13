using FluentValidation;
using FluentValidation.AspNetCore;
using HouseKeeperApi;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using HouseKeeperApi.Models.Validators;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddDbContext<HouseKeeperDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<HouseSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IHouseService, HouseService>();

// Dodanie CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")  // Adres frontendowy
                   .AllowAnyHeader()                     // Zezwolenie na dowolne nag��wki
                   .AllowAnyMethod();                    // Zezwolenie na dowolne metody (GET, POST, PUT, DELETE)
        });
});

var app = builder.Build();

// Dodanie inicjalizacji danych z RestaurantSeeder
using (var scope = app.Services.CreateScope())// Tworzymy nowy zakres (scope) dla us�ug DI
{
    var seeder = scope.ServiceProvider.GetService<HouseSeeder>();// Uzyskujemy instancj� RestaurantSeeder
    seeder?.Seed();//Wywo�ujemy metode Seed()
}

// W��czenie u�ywania CORS

// Configure the HTTP request pipeline.
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontendApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
