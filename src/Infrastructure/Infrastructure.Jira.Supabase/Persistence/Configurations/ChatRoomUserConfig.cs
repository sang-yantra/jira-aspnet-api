using Jira.Domain.Entities.ChatManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    public class ChatRoomUserConfig : IEntityTypeConfiguration<ChatRoomUser>
    {
        public void Configure(EntityTypeBuilder<ChatRoomUser> entity)
        {
            entity.HasKey(e => new { e.Id, e.ChatRoomId, e.UserId })
                  .HasName("CHAT_ROOM_USER_pkey");

            entity.ToTable("CHAT_ROOM_USER");

            entity.HasComment("chat room user association");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.Property(e => e.ChatRoomId).HasColumnName("CHAT_ROOM_ID");

            entity.Property(e => e.UserAvatarPath)
                .HasColumnType("character varying")
                .HasColumnName("USER_AVATAR_PATH");

            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("USER_NAME");

            entity.HasOne(d => d.ChatRoom)
                .WithMany(p => p.ChatRoomUsers)
                .HasForeignKey(d => d.ChatRoomId)
                .HasConstraintName("CHAT_ROOM_USER_CHAT_ROOM_ID_fkey");

            entity.HasOne(d => d.User)
                .WithMany(p => p.ChatRoomUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("CHAT_ROOM_USER_USER_ID_fkey");
        }
    }
}
