using Jira.Domain.Entities.ChatManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    public class ChatConfig : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {

            builder.ToTable("CHAT");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.ChatRoomId).HasColumnName("CHAT_ROOM_ID");

            builder.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("CREATED_BY");

            builder.Property(e => e.CreatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CREATED_DATETIME")
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            builder.Property(e => e.Message)
                .HasColumnType("character varying")
                .HasColumnName("MESSAGE");

            builder.Property(e => e.UpdatedBy)
                .HasColumnType("character varying")
                .HasColumnName("UPDATED_BY");

            builder.Property(e => e.UpdatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("UPDATED_DATETIME")
                .HasDefaultValueSql("(now() AT TIME ZONE 'utc'::text)");

            builder.Property(e => e.UserId).HasColumnName("USER_ID");

            builder.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("USER_NAME");

            builder.HasOne(d => d.ChatRoom)
                .WithMany(p => p.Chats)
                .HasForeignKey(d => d.ChatRoomId)
                .HasConstraintName("CHAT_CHAT_ROOM_ID_fkey");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Chats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("CHAT_USER_ID_fkey");
        }
    }
}
