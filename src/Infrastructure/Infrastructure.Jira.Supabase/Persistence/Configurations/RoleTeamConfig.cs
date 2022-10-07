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
    public class RoleTeamConfig: IEntityTypeConfiguration<RoleTeam>
    {
        public void Configure(EntityTypeBuilder<RoleTeam> builder)
        {
            builder.ToTable("ROLE_TEAM");

            builder.HasComment("ROLES OF A TEAM");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.RoleId).HasColumnName("ROLE_ID");

            builder.Property(e => e.TeamId).HasColumnName("TEAM_ID");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.RoleTeams)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("ROLE_TEAM_ROLE_ID_fkey");

            builder.HasOne(d => d.Team)
                .WithMany(p => p.RoleTeams)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("ROLE_TEAM_TEAM_ID_fkey");
        }
    }
}
