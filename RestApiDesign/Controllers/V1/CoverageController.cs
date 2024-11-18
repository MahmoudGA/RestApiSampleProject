using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiDesign.DTOs;

namespace RestApiDesign.Controllers.V1
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/coverage")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CoverageController : ControllerBase
    { 
        public CoverageController()
        {
        }
        [MapToApiVersion("1.0")]
        [HttpGet("facility/{facilityNumber}")]
        public async Task<IActionResult> Get(string facilityNumber)
        {
            return Ok(new { Version = "1.0", Facility = facilityNumber, Message = "This is version 1" });
        }
    }
}
