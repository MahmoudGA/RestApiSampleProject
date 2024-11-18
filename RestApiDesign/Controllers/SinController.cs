using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiDesign.DTOs;
using RestApiDesign.Models;
using RestApiDesign.Services.Interfaces;
using System.Diagnostics;

namespace RestApiDesign.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SinController : ControllerBase
    {
        private readonly ILogger<SinController> _logger;
        private readonly ISinService _sinService;
        private readonly IApiResponseFactory _apiResponseFactory;

        public SinController(ILogger<SinController> logger, ISinService sinService, IApiResponseFactory apiResponseFactory)
        {
            _logger = logger;
            _sinService = sinService;
            _apiResponseFactory = apiResponseFactory;
        }
        /*
        [HttpGet("/{sin?}")]
        public async Task<IActionResult> Get(string sin = "")
        {
            _logger.LogInformation("SinController get sin information: {sin}", sin);

            bool sinExists = await _sinService.Exists(sin);
            
            if (!sinExists)
                return BadRequest("sin doesn't exist");
            
            var sinDetails = await _sinService.GetSin(sin);

            if (sinDetails == null)
            {
                _logger.LogWarning("SIN not found: {sin}", sin);
                return NotFound(new { Message = $"SIN {sin} not found" });
            }

            return Ok(sinDetails);
        }
        
        [HttpGet]
        [Route("/{Sin?}")]
        public async Task<IActionResult> Get([FromRoute] SinRequestDTO sinDto)
        {
            _logger.LogInformation("SinController get sin information: {sin}", sinDto.Sin);

            var sinDetails = await _sinService.GetSin(sinDto.Sin);

            if (sinDetails == null)
            {
                _logger.LogWarning("SIN not found: {sin}", sinDto.Sin);
                return NotFound(new { Message = $"SIN {sinDto.Sin} not found" });
            }

            var successResponse = new ApiResponse<SinDto>
            {
                Data = sinDetails
            };

            return Ok(successResponse);
        }
        */
        [HttpGet]
        [Route("{Sin?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] SinRequestDTO sinDto)
        {
            _logger.LogInformation("Fetching SIN details for: {sin}", sinDto.Sin);

            var sinDetails = await _sinService.GetSin(sinDto.Sin);

            var successResponse = _apiResponseFactory.CreateSuccessResponse(sinDetails);
            return Ok(successResponse);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateSinDto sinDto)
        {
            await _sinService.Create(sinDto);
            return Ok();
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery] SinQueryParamsDto queryParams)
        {
            _logger.LogInformation("Received request to search for SIN: {Sin}, Status: {Status}", queryParams.Sin, queryParams.Status); 

            var searchResults = await _sinService.Search(queryParams.Sin, queryParams.Status);

            if (!searchResults.Any() || searchResults.Count() == 0)
            {
                _logger.LogWarning($"WARNING: no search results found for {queryParams.Sin} and {queryParams.Status}.");
                //return NotFound();
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new EmptyResult();
            }

            _logger.LogDebug("DEBUG: Results found for request to search for SIN: {Sin}, Status: {Status}, total results: {total}", queryParams.Sin, queryParams.Status, searchResults.Count());

            var successResponse = new ApiResponse<object>
            {
                Data = searchResults
            };

            return Ok(successResponse);
        }
    }
}
