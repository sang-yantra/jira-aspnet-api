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
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("ROLE");

            builder.HasComment("ROLE TABLE");

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

            builder.Property(e => e.Name)
                        .HasColumnType("character varying")
                        .HasColumnName("NAME");

            builder.Property(e => e.UpdatedBy)
                        .HasColumnType("character varying")
                        .HasColumnName("UPDATED_BY");

            builder.Property(e => e.UpdatedDatetime)
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("UPDATED_DATETIME")
                        .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");
        }


       
    }
}
