using GameServerSP.Application.Services.WebSocketManagement.Connections;
using System.Net.WebSockets;

namespace GameServerSP.Application.Services.WebSocketManagement.Handlers
{
    public interface IWebSocketHandler
    {
        IConnectionManager ConnectionManager { get; }
        Guid Connect(WebSocket socket);
        Task Disconnect(Guid webSocketId, WebSocket webSocket, WebSocketCloseStatus closeStatus, string closeStatusDescription);
        Task SendMessageAsync<T>(Guid socketId, T message);
        Task SendMessageAsync<T>(WebSocket socket, T message);
    }
}