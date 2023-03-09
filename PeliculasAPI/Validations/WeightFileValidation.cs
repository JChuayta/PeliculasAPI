using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validations
{
    public class WeightFileValidation : ValidationAttribute
    {
        private readonly int weightMaxInMegaBytes;

        public WeightFileValidation(int weightMaxInMegaBytes)
        {
            this.weightMaxInMegaBytes = weightMaxInMegaBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null) return ValidationResult.Success;

            if (formFile.Length > weightMaxInMegaBytes * 1024 * 1024) 
                return new ValidationResult($"El peso del archivo no debe ser mayor a {weightMaxInMegaBytes}mb");

            return ValidationResult.Success;
        }

    }
}
