using Driver.DTOs.UserDTos.Admination;
using Driver.DTOs.UserDTos.Login;
using Driver.DTOs.UserDTos.Register;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Driver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region Fields
        private readonly IAuthService _authService;
        #endregion

        #region Ctor
        public AdminController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Hanlde Functions
        [HttpGet("[action]")]
        public async Task<IActionResult> ListOfUsers()
        {
            var response = await _authService.ListOfUsers();
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ActivationUser(UserActivation activation)
        {
            var response = await _authService.UserActivation(activation);
            if(response == "Not Exist")  return BadRequest("No User With This id !"); 
            return Ok(response);
        }


        #endregion
    }
}
