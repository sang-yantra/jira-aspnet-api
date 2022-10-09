using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    internal class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {

        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("USER_ROLE");

            builder.HasComment("USER AND ROLE ASSOCIATION");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("CREATED_BY");

            builder.Property(e => e.CreatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CREATED_DATETIME")
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            builder.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("DESCRIPTION");

            builder.Property(e => e.RoleId).HasColumnName("ROLE_ID");

            builder.Property(e => e.RoleOrder).HasColumnName("ROLE_ORDER");

            builder.Property(e => e.UpdatedBy)
                .HasColumnType("character varying")
                .HasColumnName("UPDATED_BY");

            builder.Property(e => e.UpdatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("UPDATED_DATETIME")
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            builder.Property(e => e.UserId).HasColumnName("USER_ID");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("USER_ROLE_ROLE_ID_fkey");

            builder.HasOne(d => d.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("USER_ROLE_USER_ID_fkey");
        }
    }
}
