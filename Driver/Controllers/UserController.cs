using Driver.DTOs.UserDriver;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Driver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRequestDriveService _requestDriveService;
        private readonly IUserService _userService;
        private readonly ITripService _tripService;
        

        public UserController(IRequestDriveService requestDriveService, IUserService userService, ITripService tripService
            )
        {
            _requestDriveService = requestDriveService;
            _userService = userService;
            _tripService = tripService;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> SendRequest(RequestDriverDTO dto)
        {
            RequestDriverResponseDTO isBusy = await _requestDriveService.AddRequest(dto);
            return Ok(isBusy);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Complete_Trip(int tripID)
        {
            await _tripService.CompleteTrip(tripID);
            return Ok("Success");
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Get_All_Available_Drivers(string? CarType, bool isSmoking)//region
        {
            var response = await _userService.GetAllAvailableDrivers(isSmoking, CarType);
            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get_Driver_Details(string DriverID)
        {
            var response = await _userService.GetDriverDetails(DriverID);

            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddRate(string DriverID, int Rate)
        {
           await _userService.AddRate(DriverID, Rate);

            return Ok("Success");
        }

    }
}
