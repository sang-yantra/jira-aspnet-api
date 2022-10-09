using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    internal class UserRoleTeamConfig : IEntityTypeConfiguration<UserRoleTeam>
    {

        public void Configure(EntityTypeBuilder<UserRoleTeam> builder)
        {
            builder.ToTable("USER_ROLE_TEAM");

            builder.HasComment("USER ROLE TEAM ASSOCIATION");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.TeamId).HasColumnName("TEAM_ID");

            builder.Property(e => e.UserId).HasColumnName("USER_ID");

            builder.Property(e => e.UserRoleId).HasColumnName("USER_ROLE_ID");

            builder.HasOne(d => d.Team)
                .WithMany(p => p.UserRoleTeams)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("USER_ROLE_TEAM_TEAM_ID_fkey");

            builder.HasOne(d => d.User)
                .WithMany(p => p.UserRoleTeams)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("USER_ROLE_TEAM_USER_ID_fkey");

            builder.HasOne(d => d.UserRole)
                .WithMany(p => p.UserRoleTeams)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("USER_ROLE_TEAM_USER_ROLE_ID_fkey");
        }
    }
}
