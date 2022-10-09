using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    public class UserStoryConfig: IEntityTypeConfiguration<UserStory>
    {
        public void Configure(EntityTypeBuilder<UserStory> builder)
        {
            builder.ToTable("USER_STORY");

            builder.HasComment("user stories of a sprint");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.AcceptanceCriteria)
                .HasColumnType("character varying")
                .HasColumnName("ACCEPTANCE_CRITERIA");

            builder.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("CREATED_BY")
                .HasDefaultValueSql("'testUser1'::character varying");

            builder.Property(e => e.CreatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CREATED_DATETIME")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("DESCRIPTION");

            builder.Property(e => e.Effort).HasColumnName("EFFORT");

            builder.Property(e => e.Nfr)
                .HasColumnType("character varying")
                .HasColumnName("NFR");

            builder.Property(e => e.Priority)
                .HasColumnType("character varying")
                .HasColumnName("PRIORITY");

            builder.Property(e => e.SprintId).HasColumnName("SPRINT_ID");

            builder.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("STATUS");

            builder.Property(e => e.TeamId).HasColumnName("TEAM_ID");

            builder.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("TITLE");

            builder.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("TYPE");

            builder.Property(e => e.UpdatedBy)
                .HasColumnType("character varying")
                .HasColumnName("UPDATED_BY")
                .HasDefaultValueSql("'testUser1'::character varying");

            builder.Property(e => e.UpdaterdDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("UPDATERD_DATETIME")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.UserId).HasColumnName("USER_ID");

            builder.HasOne(d => d.Sprint)
                .WithMany(p => p.UserStories)
                .HasForeignKey(d => d.SprintId)
                .HasConstraintName("USER_STORY_SPRINT_ID_fkey");

            builder.HasOne(d => d.Team)
                .WithMany(p => p.UserStories)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("USER_STORY_TEAM_ID_fkey");
        }
    }
}
