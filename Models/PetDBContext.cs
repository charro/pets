using Microsoft.EntityFrameworkCore;

namespace pets.Models
{
    public class PetDBContext : DbContext
    {
        public PetDBContext(DbContextOptions<PetDBContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Animal> Animals { get; set; }
    }
}