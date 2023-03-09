using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entities;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActorsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var entities = await context.Actors.ToListAsync();
            return mapper.Map<List<ActorDTO>>(entities);
        }

        [HttpGet("{id}", Name = "getActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Gender.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null) return NotFound();
            return mapper.Map<ActorDTO>(actor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreateDTO actorCreateDTO)
        {
            var entity = mapper.Map<Actor>(actorCreateDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entity);
            return new CreatedAtRouteResult("getActor", new {id = entity.Id}, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreateDTO actorCreateDTO)
        {
            var entity = mapper.Map<Actor>(actorCreateDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Actors.AnyAsync(x => x.Id == id);
            if (exist) return NotFound();
            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
            //return await Delete<Genero>(id);
        }

    }
}
