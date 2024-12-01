using HouseKeeperApi.Models;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HouseKeeperApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowFrontendApp")]  // Włącz CORS dla tego kontrolera  -- dlaczego? bo jwt
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("registerTenant")]
        public async Task<ActionResult> RegisterTenant([FromBody] RegisterTenantDto dto)
        {
            await _accountService.RegisterTenant(dto);
            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto dto)
        {
            await _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }

        [HttpGet("getById/{userId}")]
        public async Task<ActionResult<UserByIdDto>> GetUserById([FromRoute] int userId)
        {
            var user = await _accountService.GetUserByIdDto(userId);
            return user == null ? NotFound() : Ok(user);
        }


        [HttpPost("changePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var passowrdChanged = await _accountService.ChangePassword(changePasswordDto);
            return passowrdChanged ? Ok() : NotFound();
        }
        // zmiana hasła
    }
}
