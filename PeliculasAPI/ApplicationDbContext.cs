using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entities;

namespace PeliculasAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Actor> Actors { get; set; }
    }
}
