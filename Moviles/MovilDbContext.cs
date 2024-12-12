using Microsoft.EntityFrameworkCore;

namespace Moviles
{
    public class MovilDbContext : DbContext
    {
        public MovilDbContext(DbContextOptions<MovilDbContext> options) : base(options)
        {
        }

        public DbSet<Movil> Moviles { get; set; }
    }
}
