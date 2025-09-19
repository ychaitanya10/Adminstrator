
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Adminstrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔒 This attribute requires a valid JWT for access
    public class ProtectedController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProtectedData()
        {
            // This action will only be executed if the request has a valid JWT
            return Ok("This is protected data. You must be logged in to see this.");
        }
    }
}