using Driver.DTOs.UserDTos.Admination;
using Driver.DTOs.UserDTos.Login;
using Driver.DTOs.UserDTos.Register;
using Driver.Helpers;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITripService _tripService;
        private readonly IUserService _userService;

        #endregion

        #region Ctor
        public AdminController(IAuthService authService, ITripService tripService, IUserService userService)
        {
            _authService = authService;
            _tripService = tripService;
            _userService = userService;
        }
        #endregion

        #region Hanlde Functions
        [HttpGet("[action]")]
        [Authorize(Roles =Constants.AdminRole)]
        public async Task<IActionResult> List_Of_Requested_Users()
        {
            var response = await _authService.ListOfUsers();
            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> Activation_User(UserActivation activation)
        {
            var response = await _authService.UserActivation(activation);
            if (response == "Not Exist") return BadRequest("No User With This id !");
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> GetAllTrips()
        {
            var response = await _tripService.GetAllTrips();
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> Get_Low_Rate_Drivers()
        {
            var response = await _userService.GetLowRateDrivers();
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> Get_Blocked_Drivers()
        {
            var response = await _userService.GetBlockedDrivers();
            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> BlockDriver(string DriverID)
        {
            var response = await _userService.BlockDriver(DriverID);
            return Ok(response);
        }
        [HttpPost("[action]")]
        [Authorize(Roles = Constants.AdminRole)]
        public async Task<IActionResult> UnBlockDriver(string DriverID)
        {
            var response = await _userService.UnBlockDriver(DriverID);
            return Ok(response);
        }
        #endregion
    }
}
