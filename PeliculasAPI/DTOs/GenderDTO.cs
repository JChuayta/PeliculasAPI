using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class GenderDTO : GenderCreateDTO
    {
        public int Id { get; set; }
    }
}
