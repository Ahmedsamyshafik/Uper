using Driver.DTOs.Driver;
using Driver.Service.IServices;
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
        public DriverController(IAuthService authService, IRequestDriveService driveService)
        {
            _authService = authService;
            _driveService = driveService;

        }


        [HttpPost("[action]")]
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
        public async Task<IActionResult> HandleRequest(int Requestid, bool Accept)
        {
            await _driveService.HandleRequest(Requestid, Accept);
            return Ok("Success");
        }




    }
}
