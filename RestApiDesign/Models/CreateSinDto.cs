using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace RestApiDesign.Models
{
    public class CreateSinDto
    {
        [Required]
        [StringLength(8, ErrorMessage = "Invalid sin length")]
        public string Sin { get; set; }
        [Required]
        [StringLength(14,  ErrorMessage = "Invalid national id length", MinimumLength = 14)]
        public string NationalId { get; set; }
        public string FullName { get; set; }
    }
}
