using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stronger.Domain.Entities;

namespace Stronger.Infrastructure.Configuration;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.Property(x => x.Role)
            .HasConversion<String>()
            .IsRequired();
    }
}
