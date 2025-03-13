using Cybergames.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cybergames.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<UserGame> UserGames { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the many-to-many relationship
            builder.Entity<UserGame>()
                .HasKey(ug => new { ug.UserId, ug.GameId });

            builder.Entity<UserGame>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGames)
                .HasForeignKey(ug => ug.UserId);

            builder.Entity<UserGame>()
                .HasOne(ug => ug.Game)
                .WithMany()
                .HasForeignKey(ug => ug.GameId);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "ADMIN";

            var client = new IdentityRole("client");
            client.NormalizedName = "CLIENT";

            builder.Entity<IdentityRole>().HasData(admin, client);
        }
    }
}