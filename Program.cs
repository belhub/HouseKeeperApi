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

//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//});

builder.Services.AddScoped<HouseSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IEqiupmentService, EqiupmentService>();
builder.Services.AddScoped<IHouseService, HouseService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ITrasnsactionService, TrasnsactionService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// Sprawdzanie grafików po dacie i automatyczna rotacja
builder.Services.AddHostedService<CheckScheduleBackgroundService>();

// Dodanie CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")  // Adres frontendowy
                   .AllowAnyHeader()                     // Zezwolenie na dowolne nag³ówki
                   .AllowAnyMethod();                    // Zezwolenie na dowolne metody (GET, POST, PUT, DELETE)
        });
});

var app = builder.Build();

// Dodanie inicjalizacji danych z HouseSeeder
using (var scope = app.Services.CreateScope())// Tworzymy nowy zakres (scope) dla us³ug DI
{
    var seeder = scope.ServiceProvider.GetService<HouseSeeder>();// Uzyskujemy instancjê HouseSeeder
    seeder?.Seed();//Wywo³ujemy metode Seed()
    await seeder?.RotateSchedule();
}

// Configure the HTTP request pipeline.
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontendApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
