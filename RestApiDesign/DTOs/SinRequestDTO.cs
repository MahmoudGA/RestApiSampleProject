using RestApiDesign.Validations;
using System.ComponentModel.DataAnnotations;

namespace RestApiDesign.DTOs
{
    public class SinRequestDTO
    {
        [SinValidation]
        public string Sin { get; set; }
    }
}
