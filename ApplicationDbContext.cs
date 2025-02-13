using Microsoft.EntityFrameworkCore;

namespace Cybergames
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your DbSet properties here
        // For example:
        // public DbSet<YourModel> YourModels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure your entity relationships, indexes, etc. here if needed
        }
    }
}
