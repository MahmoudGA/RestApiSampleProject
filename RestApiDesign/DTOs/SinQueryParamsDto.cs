using System.ComponentModel.DataAnnotations;

namespace RestApiDesign.DTOs
{
    public class SinQueryParamsDto
    {
        [Required(ErrorMessage = "SIN is required.")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "SIN must be a 4-digit number.")]
        public string Sin { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Active|Inactive)$", ErrorMessage = "Status must be either 'Active' or 'Inactive'.")]
        public string Status { get; set; }
    }
}
