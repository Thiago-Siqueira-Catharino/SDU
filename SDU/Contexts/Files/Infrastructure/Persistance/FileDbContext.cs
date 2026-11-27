using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Files.Domain.Entities;
using File = SDU.Contexts.Files.Domain.Entities.File;

namespace SDU.Contexts.Files.Infrastructure.Persistance;

public class FileDbContext : DbContext
{
    public DbSet<File> Files { get; set; }
    
    public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<File>()
            .HasKey(file => file.Id);
        
        modelBuilder.Entity<File>()
            .Property<Guid>("_citizenId")
            .HasColumnName("CitizenId")
            .IsRequired();
            
        modelBuilder.Entity<File>()
            .Property<Guid>("_uploaderId")
            .HasColumnName("UploaderId")
            .IsRequired();

        modelBuilder.Entity<File>()
            .HasDiscriminator<string>("FileType")
            .HasValue<Diagnostic>("Diagnostic")
            .HasValue<Exam>("Exam");
        
        base.OnModelCreating(modelBuilder); 
    }
}