using RestApiDesign.DTOs;
using RestApiDesign.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RestApiDesign.Validations
{
    public class SinValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string sin = value.ToString();

            var sinService = (ISinService)validationContext.GetService(typeof(ISinService));

            if (sinService == null || !sinService.Exists(sin).Result)
            {
                return new ValidationResult($"SIN does not exist.");
            }

            return ValidationResult.Success;
        }
    }
}
