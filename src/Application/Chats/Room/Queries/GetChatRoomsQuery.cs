using Admin.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chats.Room.Queries
{
    public class GetChatRoomsQuery: IRequest<List<RoomDto>>
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }

    public class GetChatRoomsQueryHandler : IRequestHandler<GetChatRoomsQuery, List<RoomDto>>
    {
        private readonly IJiraDbContext _context;

        public GetChatRoomsQueryHandler(IJiraDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomDto>> Handle(GetChatRoomsQuery request, CancellationToken cancellationToken)
        {
            List<RoomDto> roomsResult = new List<RoomDto>();

            var chatRooms = await (from room in _context.ChatRooms
                                   join roomUser in _context.ChatRoomUsers on room.Id equals roomUser.ChatRoomId
                                   where roomUser.UserId == request.UserId
                                   select new
                                   {
                                       ChatRoomId = room.Id,
                                       ChatRoomUserID = roomUser.UserId,
                                       Title = room.Title,
                                       Description = room.Description,
                                       CreatedDateTime = room.CreatedDatetime,
                                       UpdatedDateTime = room.UpdatedDatetime,
                                   }

                             ).ToListAsync(cancellationToken);

            var distinctChatIds = chatRooms.Select(x => x.ChatRoomId).Distinct().ToList();

            var chatRoomUsers = await (
                    from roomUser in _context.ChatRoomUsers
                    join user in _context.Users on roomUser.UserId equals user.Id
                    where distinctChatIds.Contains((Guid)roomUser.ChatRoomId)
                    select new 
                    {
                        ChatRoomId = roomUser.ChatRoomId,
                        UserId = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Avatar = user.AvatarPath,
                    }
                    
                )
                .ToListAsync(cancellationToken);


            roomsResult = (from chaRoom in chatRooms
                          select new RoomDto()
                          {
                              Id = chaRoom.ChatRoomId,
                              Title = chaRoom.Title,
                              Description = chaRoom.Description,
                              CreatedDatetime = chaRoom.CreatedDateTime,
                              UpdatedDatetime = chaRoom.UpdatedDateTime,
                              Users = chatRoomUsers.Where(x => x.ChatRoomId == chaRoom.ChatRoomId)
                              .Select(x => new UserDto()
                              {
                                  Id = x.UserId,
                                  Username = x.Username,
                                  Email =x.Email,
                                  Avatar    =x.Avatar
                              }).ToList()
                          }).ToList();

            return roomsResult;
        }
    }
}
