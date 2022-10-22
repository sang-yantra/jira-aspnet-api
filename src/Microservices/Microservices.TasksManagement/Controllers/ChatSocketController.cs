using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using PusherServer;
using System.Net;
using Chats.Chat.Events;
using Chats.Chat;
using MediatR;
using Chats.Chat.Queries;
using Chats.Room;
using Chats.Room.Queries;
using Chats.Room.Commands;

namespace Microservices.TasksManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ChatSocketController : ApiControllerBase
    {
        private readonly ILogger<ChatSocketController> _logger;

        public ChatSocketController(ILogger<ChatSocketController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/chat-raw")]
        public async Task GetChatSocket([FromQuery] string username = "anup.mahato")
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                //using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                //_logger.Log(LogLevel.Information, "WebSocket connection established");

            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        [HttpGet]
        [ActionName("chats/{roomId}")]
        public async Task<ActionResult<List<ChatDto>>> GetChats([FromRoute] string roomId= "5d6e437b-a493-4042-973a-85248b018050")
        {
            return await Mediator.Send(new GetChatsOfRoom() { RoomId = new Guid(roomId) });
        }


        [HttpPost]
        [ActionName("send-message")]
        public async Task<ActionResult<ChatDto>> HelloWorld([FromBody] SendMessageEvent chatevent)
        {
            try
            {
                return await Mediator.Send(chatevent);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace.ToString());
            }
        }

        [HttpGet]
        [ActionName("rooms")]
        [ProducesResponseType(200, Type = typeof(List<RoomDto>))]
        public async Task<ActionResult<List<RoomDto>>> GetRooms([FromQuery] GetChatRoomsQuery roomsQuery)
        {
            try
            {
                return await Mediator.Send<List<RoomDto>>(roomsQuery);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace.ToString());

            }
        }

        /// <summary>
        /// Create room
        /// </summary>
        /// <example>
        /// {
        ///  "title": "new room from swagger",
        ///  "description": "new room from swagger",
        ///  "createdBy": "anupmahato033@gmail.com",
        ///  "updatedBy": "anupmahato033@gmail.com",
        ///  "chatRoomType": "ROOM",
        ///  "users": [
        ///    {
        ///      "id": "0c898fba-7086-449d-9d5d-463a4025a61e"
        ///    },
        ///    {
        ///      "id": "02547a57-3ee0-43e5-93dc-13422047e22b"
        ///    }
        ///  ]
        ///}
        /// </example>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("room")]
        [ProducesResponseType(200, Type = typeof(List<RoomDto>))]
        public async Task<ActionResult> CreateRoom([FromBody] CreateChatRoomCommand chatRoom)
        {
            try
            {
                await Mediator.Send(chatRoom);
                return Created("/chats","Resource created");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace.ToString());

            }
        }
    } 
}
