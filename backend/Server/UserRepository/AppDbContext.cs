using Microsoft.EntityFrameworkCore;
using Npgsql;
using Server.Domain;

namespace Server.UserRepository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<User>? Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(@"Host=localhost;Port=5433;Username=docker;Password=docker;Database=connect");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Username = "Fabricio",
                    Number = "38123456789",
                    Password = "123456"
                }
            );
        }
    }
}
