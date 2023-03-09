using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entities;
using System.Data;

namespace PeliculasAPI.Controllers
{

    [ApiController]
    [Route("api/generos")]
    public class GenderController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenderController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenderDTO>>> Get()
        {
            var genders = await context.Gender.ToListAsync();
            var genderList = mapper.Map<List<GenderDTO>>(genders);
            return genderList;
        }

        [HttpGet("{id:int}", Name = "getGender")]
        public async Task<ActionResult<GenderDTO>> Get(int id)
        {
            var entity = await context.Gender.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return NotFound();

            var dto = mapper.Map<GenderDTO>(entity);
            return dto;
            //return await Get<Gender, GenderDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenderCreateDTO genderCreateDTO)
        {
            var entity = mapper.Map<Gender>(genderCreateDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            var genderDTO = mapper.Map<GenderDTO>(entity);
            return new CreatedAtRouteResult("getGender", new { id = genderDTO.Id }, genderDTO);
            //return await Post<GenderCreateDTO, Gender, GenderDTO>(genderCreateDTO, "getGender");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenderCreateDTO genderCreateDTO)
        {
            var entity = mapper.Map<Gender>(genderCreateDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
            //return await Post<GenderCreateDTO, Gender, GenderDTO>(genderCreateDTO, "getGender");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Gender.AnyAsync(x => x.Id == id);
            if (exist) return NotFound();
            context.Remove(new Gender() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
            //return await Delete<Genero>(id);
        }

        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] GenderCreateDTO genderCreateDTO)
        //{
        //    return await Post<GenderCreateDTO, Gender, GenderDTO>(genderCreateDTO, "getGender");
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put(int id, [FromBody] GenderCreateDTO genderCreateDTO)
        //{
        //    return await Put<GenderCreateDTO, Gender>(id, genderCreateDTO);
        //}

        //[HttpDelete("{id}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    return await Delete<Genero>(id);
        //}
    }
}
