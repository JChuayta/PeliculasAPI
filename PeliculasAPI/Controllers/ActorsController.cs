using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entities;
using WebApiAutores.Servicios;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStore almacenadorArchivos;
        private readonly string container = "actors";

        public ActorsController(ApplicationDbContext context, IMapper mapper, IFileStore almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
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
            var entidad = mapper.Map<Actor>(actorCreateDTO);

            if (actorCreateDTO.Photo != null)
            {
                using var memoryStream = new MemoryStream();
                await actorCreateDTO.Photo.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                var extension = Path.GetExtension(actorCreateDTO.Photo.FileName);
                entidad.Photo = await almacenadorArchivos.SaveFile(contenido, extension, container,
                    actorCreateDTO.Photo.ContentType);
            }

            context.Add(entidad);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entidad);
            return new CreatedAtRouteResult("getActor", new { id = entidad.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreateDTO actorCreateDTO)
        {
            var actorDB = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);

            if (actorDB == null) { return NotFound(); }

            actorDB = mapper.Map(actorCreateDTO, actorDB);

            if (actorCreateDTO.Photo != null)
            {
                using var memoryStream = new MemoryStream();
                await actorCreateDTO.Photo.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                var extension = Path.GetExtension(actorCreateDTO.Photo.FileName);
                actorDB.Photo = await almacenadorArchivos.EditFile(contenido, extension, container,
                    actorDB.Photo,
                    actorCreateDTO.Photo.ContentType);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();
            var entityDB = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entityDB == null) return NotFound();
            var entityDTO = mapper.Map<ActorPatchDTO>(entityDB);
            patchDocument.ApplyTo(entityDTO);
            var isValid = TryValidateModel(ModelState);
            if(!isValid) return BadRequest(ModelState);
            mapper.Map(entityDTO, entityDB);
            await context.SaveChangesAsync();
            return NoContent();
            //return await Patch<Actor, ActorPatchDTO>(id, patchDocument);
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
