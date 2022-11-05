using Admin.Users;

namespace Chats.Room.Commands
{
    public class CreateChatRoomCommand : IRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ChatRoomType { get; set; }
        public List<UserDto> Users { get; set; }
    }

    public class CreateChatRoomCommandHandler : IRequestHandler<CreateChatRoomCommand, Unit>
    {
        private readonly IJiraDbContext _context;
        public CreateChatRoomCommandHandler(IJiraDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(CreateChatRoomCommand request, CancellationToken cancellationToken)
        {
            RoomDto chatRoomRespose = new RoomDto();

            Guid ChatRoomId = Guid.NewGuid();

            ChatRoom chatRoom = new ChatRoom()
            {
                Id = ChatRoomId,
                Title = request.Title,
                Description = request.Description,
                ImagePath = request.ImagePath,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
                ChatRoomType = request.ChatRoomType,
                ChatRoomUsers = request.Users.Select(x => new ChatRoomUser()
                {
                    Id = Guid.NewGuid(),
                    ChatRoomId = ChatRoomId,
                    UserId = x.Id,
                }).ToList(),
            };

            _context.ChatRooms.Add(chatRoom);
            await _context.SaveChangesAsync(cancellationToken);
            return new Unit();
        }

    }
}
