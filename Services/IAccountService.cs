using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface IAccountService
    {
        string GenerateJwt(LoginUserDto dto);
        void RegisterUser(RegisterUserDto registerUserDto);
    }
}