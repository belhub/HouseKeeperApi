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
        private readonly AuthenticationSettings _authenticationSettings;
        public AccountService(HouseKeeperDbContext houseKeeperDbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _houseKeeperDbContext = houseKeeperDbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
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
    }
}
