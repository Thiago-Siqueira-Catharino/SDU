using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Infrastructure.Persistance;

public class AuthDbContext : IdentityDbContext<User>
{
    public DbSet<Citizen> Citizens { get; set; }
    public DbSet<Professional> Professionals { get; set; }
    public DbSet<Entity>  Entities { get; set; }
    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}