using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestApiDesign.Controllers.V2
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/coverage")]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CoverageController : ControllerBase
    {
        public CoverageController()
        {
        }
        [MapToApiVersion("2.0")]
        [HttpGet("facility/{facilityNumber}")]
        public async Task<IActionResult> Get(string facilityNumber)
        {
            return Ok(new { Version = "2.0", Facility = facilityNumber, Message = "This is version 2" });
        }
    }
}
