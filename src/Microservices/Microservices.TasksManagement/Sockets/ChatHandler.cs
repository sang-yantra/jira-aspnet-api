namespace Microservices.TasksManagement.Sockets
{
    public class ChatHandler : WebSocketHandler
    {
        public ChatHandler(ConnectionManager connectionManager) : base(connectionManager)
        {
        }
    }
}
