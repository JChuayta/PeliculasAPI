using AutoMapper;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entities;

namespace PeliculasAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderCreateDTO, Gender>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreateDTO, Actor>().ReverseMap();
        }
    }
}
