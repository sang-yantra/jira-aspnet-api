using Jira.Domain.Entities.ChatManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Jira.Supabase.Persistence.Configurations
{
    public class ChatRoomConfig : IEntityTypeConfiguration<ChatRoom>
    {
        public void Configure(EntityTypeBuilder<ChatRoom> builder)
        {

            builder.ToTable("CHAT_ROOM");

            builder.HasComment("table for chat room");

            builder.Property(e => e.Id)
                .HasColumnName("ID")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.CreatedBy)
                .HasColumnType("character varying")
                .HasColumnName("CREATED_BY");

            builder.Property(e => e.CreatedDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CREATED_DATETIME")
                .HasDefaultValueSql("now()");

            builder.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("DESCRIPTION");

            builder.Property(e => e.ImagePath)
                .HasColumnType("character varying")
                .HasColumnName("IMAGE_PATH");

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

            builder.Property(e => e.ChatRoomType)
                .HasColumnType("character varying")
                .HasColumnName("CHAT_ROOM_TYPE")
                .HasDefaultValueSql("'ROOM'::character varying");
        }
    }
}
