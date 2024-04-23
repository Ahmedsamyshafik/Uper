using Driver.DTOs.UserDTos.Login;
using Driver.DTOs.UserDTos.Register;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Driver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Fields
        private readonly IAuthService _authService;
        #endregion

        #region Ctor
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Hanlde Functions
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO DTO)
        {
            var result = await _authService.RegisterAsync(DTO);
            if (result.message == "Existing") return BadRequest(new { code = result.message, message = result.error });
            if (result.message == "Password") return BadRequest(new { code = result.message, message = result.error });
            if (result.message == "Image") return BadRequest(new { code = result.message, message = result.error });
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDTO DTO)
        {
            var result = await _authService.Login(DTO);
            if (!result.IsAuthenticated) return BadRequest(result.Message);
            return Ok(result);
        }
        #endregion
    }
}
