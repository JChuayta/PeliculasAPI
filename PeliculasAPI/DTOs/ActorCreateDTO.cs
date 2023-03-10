using PeliculasAPI.Enums;
using PeliculasAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class ActorCreateDTO : ActorPatchDTO
    {
        [WeightFileValidation(weightMaxInMegaBytes: 4)]
        [TypeFileValidation(groupTypeFile: GroupTypeFile.Image)]
        public IFormFile Photo { get; set; }
    }
}
