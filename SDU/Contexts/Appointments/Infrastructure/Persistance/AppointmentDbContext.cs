using Microsoft.EntityFrameworkCore;
using SDU.Contexts.Appointments.Domain.Entities;

namespace SDU.Contexts.Appointments.Infrastructure.Persistance;

public class AppointmentDbContext : DbContext
{
    public DbSet<Appointment> Appointments { get; set; }

    public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>()
            .HasKey(a => a.Id);
        
        modelBuilder.Entity<Appointment>()
            .Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue(AppointmentStatus.Agendado);

        base.OnModelCreating(modelBuilder);
    }
}