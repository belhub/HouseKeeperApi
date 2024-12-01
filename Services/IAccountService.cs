using HouseKeeperApi.Models;

namespace HouseKeeperApi.Services
{
    public interface IAccountService
    {
        string GenerateJwt(LoginUserDto dto);
        Task RegisterUser(RegisterUserDto registerUserDto);
        //int GetUserId();
        Task<UserByIdDto> GetUserByIdDto(int userId);
        Task<bool> ChangePassword(ChangePasswordDto changePasswordDto);
        Task RegisterTenant(RegisterTenantDto registerUserDto);
    }
}