using PeliculasAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Validations
{
    public class TypeFileValidation : ValidationAttribute
    {
        private readonly string[] typeValidates;

        public TypeFileValidation(string[] typesValidate)
        {
            this.typeValidates = typesValidate;
        }

        public TypeFileValidation(GroupTypeFile groupTypeFile)
        {
            if (groupTypeFile == GroupTypeFile.Image)
            {
                typeValidates = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null) return ValidationResult.Success;

            if (!typeValidates.Contains(formFile.ContentType)) 
                return new ValidationResult($"El tipo del archivo debe ser uno de los siguientes: {string.Join(", ", typeValidates)}");

            return ValidationResult.Success;
        }
    }
}
