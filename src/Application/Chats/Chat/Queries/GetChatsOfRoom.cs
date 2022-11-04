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
            var chats = await (from chat in _context.Chats
                         join user in _context.Users on chat.UserId equals user.Id
                         where chat.ChatRoomId == request.RoomId
                         select new ChatDto()
                         {
                             Id = chat.Id,
                             ChatRoomId = chat.ChatRoomId,
                             UserId = chat.UserId,
                             UserName = chat.UserName,
                             Avatar = user.AvatarPath,
                             Message = chat.Message,
                             CreatedBy = chat.CreatedBy,
                             CreatedDatetime = chat.CreatedDatetime,
                             UpdatedBy = chat.UpdatedBy,
                             UpdatedDatetime = chat.UpdatedDatetime,
                         })
                         .OrderBy(x => x.CreatedDatetime)
                         .ToListAsync(cancellationToken);
            return chats;
        }
    }

}
