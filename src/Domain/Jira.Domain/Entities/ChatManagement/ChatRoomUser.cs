using Jira.Domain.Entities.ProjectManagement;

namespace Jira.Domain.Entities.ChatManagement
{
    /// <summary>
    /// chat room user association
    /// </summary>
    public partial class ChatRoomUser
    {
        public Guid Id { get; set; }
        public Guid? ChatRoomId { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatarPath { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual User User { get; set; }
    }
}
