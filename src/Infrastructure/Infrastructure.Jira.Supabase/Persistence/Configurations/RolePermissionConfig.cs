using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    public class RolePermissionConfig : IEntityTypeConfiguration<RolePermission>
    {

        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {

            builder.ToTable("ROLE_PERMISSION");

            builder.HasComment("ROLE AND PERMISSION ASSOCIATION");

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

            builder.Property(e => e.PermissionId).HasColumnName("PERMISSION_ID");

            builder.Property(e => e.RoleId).HasColumnName("ROLE_ID");

            builder.Property(e => e.UpdatedBy)
                .HasColumnType("character varying")
                .HasColumnName("UPDATED_BY");

            builder.Property(e => e.UpdatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("UPDATED_DATETIME")
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            builder.HasOne(d => d.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("ROLE_PERMISSION_PERMISSION_ID_fkey");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("ROLE_PERMISSION_ROLE_ID_fkey");
        }


    }
}
