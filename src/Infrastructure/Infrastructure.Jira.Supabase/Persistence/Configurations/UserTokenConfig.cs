using Jira.Domain.Entities.ProjectManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    internal class UserTokenConfig : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> entity)
        {
            entity.ToTable("USER_TOKEN");

            entity.HasComment("token managmenr for user");

            entity.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            entity.Property(e => e.AccessToken)
                .HasColumnType("character varying")
                .HasColumnName("ACCESS_TOKEN");

            entity.Property(e => e.AccessTokenExpired).HasColumnName("ACCESS_TOKEN_EXPIRED");

            entity.Property(e => e.RefreshToken)
                .HasColumnType("character varying")
                .HasColumnName("REFRESH_TOKEN");

            entity.Property(e => e.RefreshTokenExpired).HasColumnName("REFRESH_TOKEN_EXPIRED");

            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("USER_TOKEN_USER_ID_fkey");
        }
    }
}
