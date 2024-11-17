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

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }

        [HttpGet("getById{userId}")]
        public ActionResult GetUserById([FromRoute] int userId)
        {
            var user = _accountService.GetUserByIdDto(userId);
            return user == null ? NotFound() : Ok(user);
        }
        //wyszukiwanie usera po id
        //zmiana hasła
    }
}
