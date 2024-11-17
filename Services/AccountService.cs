using AutoMapper;
using HouseKeeperApi.Entities;
using HouseKeeperApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HouseKeeperApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly HouseKeeperDbContext _houseKeeperDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IMapper _mapper;
        public AccountService(IMapper mapper, HouseKeeperDbContext houseKeeperDbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IHttpContextAccessor httpContextAccessor)
        {
            _houseKeeperDbContext = houseKeeperDbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }

        public async Task<UserByIdDto> GetUserByIdDto(int userId)
        {
            try
            {
                var user = await _houseKeeperDbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId)
                    ?? throw new Exception($"Brak usera o Id = {userId}.");

                return _mapper.Map<UserByIdDto>(user);
            }
            catch (DbUpdateException ex) { throw new InvalidOperationException("Wystapil problem podczas pobierania usera .", ex); }
            catch (Exception ex) { throw new Exception("Wystapil nieoczekiwany blad podczas pobierania usera.", ex); }
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = new User()
            {
                Name = registerUserDto.Name,
                Surname = registerUserDto.Surname,
                Email = registerUserDto.Email,
                Phone = registerUserDto.Phone,
                DateOfBirth = registerUserDto.DateOfBirth,
                RoleId = registerUserDto.RoleId,
            };


            var passwordHash = _passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = passwordHash;
            _houseKeeperDbContext.Users.Add(newUser);
            _houseKeeperDbContext.SaveChanges();
        }
        public string GenerateJwt(LoginUserDto loginUserDto)
        {
            var user = _houseKeeperDbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == loginUserDto.Email) ?? throw new Exception("Invalid Username or password");
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password); //czy poprawne hasło czy nie

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid Username or password");

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.Name}"),
                new(ClaimTypes.Surname, $"{user.Surname }"),
                new(ClaimTypes.Role, $"{user.Role.RoleName}"), //dzięki temu odczytujemy role użytkownika
                new(ClaimTypes.Email, $"{user.Email}"),
                new(ClaimTypes.MobilePhone,  $"{user.Phone}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)); //klucz prywatny

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //kluc pplus algorytm hashowania

            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public int GetUserId()
        {
            // Pobieranie UserId z tokena JWT (dla JWT zazwyczaj jest przechowywane jako "sub")
            var userId = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? int.Parse(userId) : throw new UnauthorizedAccessException("UserId not found");
        }
    }
}
