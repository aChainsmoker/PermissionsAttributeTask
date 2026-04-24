using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PermissionsAttributeTask.DataAccess.Entities;

namespace PermissionsAttributeTask.DataAccess.Configurations;

public class UserProfileConfigurations : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        builder.HasKey(x => x.Id);
        builder
            .Property(x=>x.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(x=>x.LastName)
            .IsRequired()
            .HasMaxLength(100);
        builder
            .Property(x=>x.Age) 
            .IsRequired();
        builder
            .Property(x=>x.About)
            .IsRequired(false)
            .HasMaxLength(500);
    }
}