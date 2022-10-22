using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USER");

            builder.HasComment("USER TABLE");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.Alias)
                .HasColumnType("character varying")
                .HasColumnName("ALIAS");

            builder.Property(e => e.AvatarPath)
                .HasColumnType("character varying")
                .HasColumnName("AVATAR_PATH");

            builder.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("CREATED_BY");

            builder.Property(e => e.CreatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CREATED_DATETIME")
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            builder.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("EMAIL");

            builder.Property(e => e.Firstname)
                .HasColumnType("character varying")
                .HasColumnName("FIRSTNAME");

            builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE");

            builder.Property(e => e.IsSuperAdmin).HasColumnName("IS_SUPER_ADMIN");

            builder.Property(e => e.Lastname)
                .HasColumnType("character varying")
                .HasColumnName("LASTNAME");

            builder.Property(e => e.Middlename)
                .HasColumnType("character varying")
                .HasColumnName("MIDDLENAME");

            builder.Property(e => e.UpdatedBy)
                .HasColumnType("character varying")
                .HasColumnName("UPDATED_BY");

            builder.Property(e => e.UpdatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("UPDATED_DATETIME")
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            builder.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("USERNAME");
        }
    }
}
