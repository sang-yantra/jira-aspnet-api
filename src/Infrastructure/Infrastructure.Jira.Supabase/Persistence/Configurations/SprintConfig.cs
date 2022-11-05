using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    public class SprintConfig : IEntityTypeConfiguration<Sprint>
    {
        public void Configure(EntityTypeBuilder<Sprint> builder)
        {
            builder.ToTable("SPRINT");

            builder.HasComment("sprint table");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.Capacity).HasColumnName("CAPACITY");

            builder.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("CREATED_BY");

            builder.Property(e => e.CreatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CREATED_DATETIME")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("END_DATE");

            builder.Property(e => e.SprintNumber).HasColumnName("SPRINT_NUMBER");

            builder.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("START_DATE");

            builder.Property(e => e.TeamId).HasColumnName("TEAM_ID");

            builder.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("TITLE");

            builder.Property(e => e.UpdatedBy)
                .HasColumnType("character varying")
                .HasColumnName("UPDATED_BY");

            builder.Property(e => e.UpdatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("UPDATED_DATETIME")
                .HasDefaultValueSql("now()");

            builder.HasOne(d => d.Team)
                .WithMany(p => p.Sprints)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("SPRINT_TEAM_ID_fkey");
        }
    }
}
