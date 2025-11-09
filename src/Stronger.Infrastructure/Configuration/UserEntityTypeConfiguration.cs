using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stronger.Domain.Entities;
using Stronger.Domain.Enums;

namespace Stronger.Infrastructure.Configuration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasKey(user => user.Id);

        builder
            .Property(user => user.CreatedAt)
            .IsRequired();

        builder
            .Property(user => user.Forename)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(user => user.Surname)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(user => user.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .HasIndex(user => user.Email)
            .IsUnique();

        builder
            .Property(user => user.Forename)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(user => user.PasswordHash)
            .IsRequired();

        builder
            .Property(user => user.Dob)
            .HasColumnType("date");

        builder
            .HasOne<RoleEntity>()
            .WithMany()
            .HasForeignKey(user => user.RoleId)
            .IsRequired();
    }
}
