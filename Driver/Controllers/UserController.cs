using Driver.DTOs.UserDriver;
using Driver.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Driver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRequestDriveService _requestDriveService;
        public UserController(IRequestDriveService requestDriveService)
        {
            _requestDriveService = requestDriveService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> SendRequest(RequestDriverDTO dto)
        {
            RequestDriverResponseDTO isBusy = await _requestDriveService.AddRequest(dto);
            return Ok(isBusy);
        }
    }
}
