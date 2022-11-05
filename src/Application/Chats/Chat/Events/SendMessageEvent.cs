using Chats.Configuration;
using Microsoft.Extensions.Configuration;
using PusherServer;

namespace Chats.Chat.Events
{
    public class SendMessageEvent : IRequest<ChatDto>
    {
        public Guid ChannelId { get; set; }
        public string Event { get; set; }
        public string Message { get; set; }

        public Guid SenderId { get; set; }
        public string SenderUsername { get; set; }

    }

    public class SendMessageEventHandler : IRequestHandler<SendMessageEvent, ChatDto>
    {
        private readonly IConfiguration _configuration;
        private readonly IJiraDbContext _context;
        private readonly PusherConfiguration _pusherConfig;
        public SendMessageEventHandler(IConfiguration configuration, IJiraDbContext context, PusherConfiguration pusherConfiguration)
        {
            _configuration = configuration;
            _context = context;
            _pusherConfig = pusherConfiguration;
        }

        public async Task<ChatDto> Handle(SendMessageEvent request, CancellationToken cancellationToken)
        {
            ChatDto chatDto = new ChatDto();
            Guid ChatId = Guid.NewGuid();
            DateTime createdDatTime = DateTime.Now;
            var options = new PusherOptions
            {
                Cluster = _pusherConfig.Cluster,
                Encrypted = true
            };

            var pusher = new Pusher(
              _pusherConfig.ApplicationId,
              _pusherConfig.Key,
              _pusherConfig.Secret,
              options);

            var result = await pusher.TriggerAsync(
              request.ChannelId.ToString(),
              request.Event,
              new ChatDtoCamelCase()
              {
                  id = ChatId,
                  chatRoomId = request.ChannelId,
                  userId = request.SenderId,
                  userName = request.SenderUsername,
                  message = request.Message,
                  createdDatetime = createdDatTime,
                  updatedDatetime = createdDatTime,
                  createdBy = request.SenderUsername,
                  updatedBy = request.SenderUsername,
              });

            Jira.Domain.Entities.ChatManagement.Chat chat = new Jira.Domain.Entities.ChatManagement.Chat()
            {
                Id = ChatId,
                ChatRoomId = request.ChannelId,
                UserId = request.SenderId,
                UserName = request.SenderUsername,
                Message = request.Message,
                CreatedDatetime = createdDatTime,
                UpdatedDatetime = createdDatTime,
                CreatedBy = request.SenderUsername,
                UpdatedBy = request.SenderUsername,
            };

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync(cancellationToken);

            chatDto = new ChatDto()
            {
                Id = chat.Id,
                ChatRoomId = chat.ChatRoomId,
                UserId = chat.UserId,
                UserName = chat.UserName,
                Message = chat.Message,
                CreatedDatetime = createdDatTime,
                UpdatedDatetime = createdDatTime,
                CreatedBy = chat.UserName,
                UpdatedBy = chat.UserName,
            };

            return chatDto;
        }
    }
}
