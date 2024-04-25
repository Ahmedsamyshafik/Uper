using Driver.DTOs.Driver;
using Driver.Helpers;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Driver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRequestDriveService _driveService;
        private readonly IUserService _userService;
        public DriverController(IAuthService authService, IRequestDriveService driveService, IUserService userService)
        {
            _authService = authService;
            _driveService = driveService;
            _userService = userService;
        }


        [HttpPost("[action]")]
        [Authorize(Roles = Constants.DriverRole)]
        public async Task<IActionResult> GetRequests(string DriverId)
        {
            //Get user from userid
            //Repo=> rec==> driverid==id
            var requests = await _driveService.GetDriverRequests(DriverId);
            //mapping
            List<AllDriverRequestedResponseDTO> response = new();
            foreach (var request in requests)
            {
                var pass = await _authService.GetUserById(request.PassingerID);
                //temp
                AllDriverRequestedResponseDTO temp = new()
                {

                    dateTime = request.DateTime,
                    name = pass.UserName,
                    price = request.price,
                    Source = request.Source,
                    Target = request.Target,
                    id = request.id,
                    ImageUrl =request.Passenger.imageUrl


                };
                response.Add(temp);
            }
            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = Constants.DriverRole)]
        public async Task<IActionResult> HandleRequest(int Requestid, bool Accept)
        {
            await _driveService.HandleRequest(Requestid, Accept);
            return Ok("Success");
        }

        [HttpGet("[action]")]
        [Authorize(Roles = Constants.DriverRole)]
        public async Task<IActionResult> Get_Driver_Total_Price(string driverID)
        {
            GetDriverPriceResponse response = new();
            response.Price= await _userService.GetDriverTotalPrice(driverID);
            return Ok(response);
        }


    }
}
