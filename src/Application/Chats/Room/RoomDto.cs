using Admin.Users;

namespace Chats.Room
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ChatRoomType { get; set; }
        public List<UserDto> Users { get; set; }
    }

}
