using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Infrastructure.Persistance;

public class AuthDbContext : IdentityDbContext<User>
{
    public DbSet<Citizen> Citizens { get; set; }
    public DbSet<Professional> Professionals { get; set; }
    public DbSet<Entity>  Entities { get; set; }
    public DbSet<CitizenFile> CitizenFiles { get; set; }
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>()
            .Property<string>("_cnpj")
            .HasColumnName("CNPJ")
            .IsRequired();
        
        modelBuilder.Entity<Professional>()
            .Property<Guid>("_entityId")
            .HasColumnName("EntityId")
            .IsRequired();
        
        modelBuilder.Entity<Professional>()
            .Property<string>("_cim")
            .HasColumnName("cim")
            .IsRequired();
        
        base.OnModelCreating(modelBuilder);
        
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}