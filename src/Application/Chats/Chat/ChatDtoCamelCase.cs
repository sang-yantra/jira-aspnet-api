using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chats.Chat
{
    public class ChatDtoCamelCase
    {
        public Guid id { get; set; }
        public Guid? chatRoomId { get; set; }
        public Guid? userId { get; set; }
        public string? userName { get; set; }
        public string? message { get; set; }
        public DateTime? createdDatetime { get; set; }
        public DateTime? updatedDatetime { get; set; }
        public string? createdBy { get; set; }
        public string? updatedBy { get; set; }
    }
}
