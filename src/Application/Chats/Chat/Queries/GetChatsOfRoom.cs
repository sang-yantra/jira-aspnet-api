using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chats.Chat.Queries
{
    public class GetChatsOfRoom: IRequest<List<ChatDto>>
    {
        public Guid RoomId { get; set; }
    }

    public class GetChatsOfRoomHandler : IRequestHandler<GetChatsOfRoom, List<ChatDto>>
    {
        private readonly IJiraDbContext _context; 

        public GetChatsOfRoomHandler(IJiraDbContext context)
        {
            _context = context; 
        }
        public async Task<List<ChatDto>> Handle(GetChatsOfRoom request, CancellationToken cancellationToken)
        {
            var chats = await _context.Chats.Where(x => x.ChatRoomId == request.RoomId)
                .Select(x => new ChatDto()
                {
                    Id = x.Id,
                    ChatRoomId = x.ChatRoomId,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Message = x.Message,
                    CreatedBy = x.CreatedBy,
                    CreatedDatetime = x.CreatedDatetime,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDatetime = x.UpdatedDatetime,
                })
                .OrderByDescending(x => x.CreatedDatetime)
                .ToListAsync(cancellationToken);
            return chats;
        }
    }

}
