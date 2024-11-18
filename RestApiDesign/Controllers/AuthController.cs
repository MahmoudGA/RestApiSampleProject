using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestApiDesign.Models;
using RestApiDesign.Services;
using RestApiDesign.Services.Interfaces;

namespace RestApiDesign.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly ISinService _sinService;

        public AuthController(JwtTokenService jwtTokenService, ISinService sinService)
        {
            _jwtTokenService = jwtTokenService;
            _sinService = sinService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                return BadRequest(new { message = "Username and password are required." });
            

            string nationalId = await _sinService.Authenticate(loginRequest.Username, loginRequest.Password);

            if (String.IsNullOrWhiteSpace(nationalId))
                return Unauthorized(new { message = "Invalid credentials." });

            var token = _jwtTokenService.GenerateToken(loginRequest.Username, nationalId);
            return Ok(new { token });
        }
    }

}
