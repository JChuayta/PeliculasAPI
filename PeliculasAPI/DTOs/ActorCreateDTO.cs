using PeliculasAPI.Enums;
using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class ActorCreateDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        [WeightFileValidation(weightMaxInMegaBytes: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile Photo { get; set; }
    }
}
