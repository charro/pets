using Microsoft.EntityFrameworkCore;

namespace pets.Models
{
    public class AnimalsContext : DbContext
    {
        public AnimalsContext(DbContextOptions<AnimalsContext> options)
            : base(options)
        {
        }
        public DbSet<Animal> Animals { get; set; }

    }
}