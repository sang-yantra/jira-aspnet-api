using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    internal class TaskInfoConfig : IEntityTypeConfiguration<TaskInfo>
    {

        public void Configure(EntityTypeBuilder<TaskInfo> builder)
        {
            builder.ToTable("TASK_INFO");

            builder.HasComment("Task table");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.AcceptanceCriteria)
                .HasColumnType("character varying")
                .HasColumnName("ACCEPTANCE_CRITERIA");

            builder.Property(e => e.Completed).HasColumnName("COMPLETED");

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

            builder.Property(e => e.Nfr)
                .HasColumnType("character varying")
                .HasColumnName("NFR");

            builder.Property(e => e.OriginalEstimate).HasColumnName("ORIGINAL_ESTIMATE");

            builder.Property(e => e.Priority)
                .HasColumnType("character varying")
                .HasColumnName("PRIORITY")
                .HasDefaultValueSql("'LOW'::character varying");

            builder.Property(e => e.Remaining).HasColumnName("REMAINING");

            builder.Property(e => e.SprintId).HasColumnName("SPRINT_ID");

            builder.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("STATUS")
                .HasDefaultValueSql("'ACTIVE'::character varying");

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
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            builder.Property(e => e.UserId).HasColumnName("USER_ID");

            builder.Property(e => e.UserStoryId).HasColumnName("USER_STORY_ID");

            builder.HasOne(d => d.Sprint)
                .WithMany(p => p.TaskInfos)
                .HasForeignKey(d => d.SprintId)
                .HasConstraintName("TASK_INFO_SPRINT_ID_fkey");

            builder.HasOne(d => d.Team)
                .WithMany(p => p.TaskInfos)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("TASK_INFO_TEAM_ID_fkey");

            builder.HasOne(d => d.UserStory)
                .WithMany(p => p.TaskInfos)
                .HasForeignKey(d => d.UserStoryId)
                .HasConstraintName("TASK_INFO_USER_STORY_ID_fkey");
        }

    }
}
