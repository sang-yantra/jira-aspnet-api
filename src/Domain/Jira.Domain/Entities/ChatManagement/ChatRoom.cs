namespace Jira.Domain.Entities.ChatManagement
{
    /// <summary>
    /// table for chat room
    /// </summary>
    public partial class ChatRoom
    {
        public ChatRoom()
        {
            ChatRoomUsers = new HashSet<ChatRoomUser>();
            Chats = new HashSet<Chat>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ChatRoomType { get; set; }

        public virtual ICollection<ChatRoomUser> ChatRoomUsers { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
    }
}
