using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json.Serialization;

namespace RestApiDesign.DTOs
{
    public class SinDto
    {
        [JsonPropertyName("sin")]
        public string SocialInsuranceNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string NationalId { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
    }
}
