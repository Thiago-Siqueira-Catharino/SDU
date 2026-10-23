using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDU.Contexts.Auth.Domain.Entities;

namespace SDU.Contexts.Auth.Infrastructure.Persistance;

public class CitizenFileConfiguration : IEntityTypeConfiguration<CitizenFile>
{
    public void Configure(EntityTypeBuilder<CitizenFile> builder)
    {
        builder.ToTable("CitizenFiles");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.StoragePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Size)
            .IsRequired();

        // Relacionamento lógico (sem navigations properties na entidade, apenas os IDs)
        builder.HasIndex(c => c.OwnerId);
        builder.HasIndex(c => c.UploaderId);
    }
}