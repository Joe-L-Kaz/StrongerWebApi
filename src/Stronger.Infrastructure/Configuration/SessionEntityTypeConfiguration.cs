using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Configuration;

public class SessionEntityTypeConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder
            .HasKey(s => s.Id);

        builder
            .Property(s => s.CompletedAt)
            .IsRequired();
        
        builder
            .HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
