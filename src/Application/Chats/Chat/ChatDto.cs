namespace Chats.Chat
{
    public class ChatDto
    {
        public Guid Id { get; set; }
        public Guid? ChatRoomId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Avatar { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
