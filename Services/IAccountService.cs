using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface IAccountService
    {
        string GenerateJwt(LoginUserDto dto);
        void RegisterUser(RegisterUserDto registerUserDto);
        int GetUserId();
        Task<UserByIdDto> GetUserByIdDto(int userId);
    }
}