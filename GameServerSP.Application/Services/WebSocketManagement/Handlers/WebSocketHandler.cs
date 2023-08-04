using GameClientServerSP.Shared;
using GameServerSP.Application.Services.WebSocketManagement.Connections;
using System.Net.WebSockets;

namespace GameServerSP.Application.Services.WebSocketManagement.Handlers;

public class WebSocketHandler : IWebSocketHandler
{
    protected readonly IConnectionManager _connectionManager;

    public WebSocketHandler(IConnectionManager webSocketCOnnectionManager)
    {
        _connectionManager = webSocketCOnnectionManager;
    }

    public IConnectionManager ConnectionManager => _connectionManager;

    public Guid Connect(WebSocket socket)
    {
        return _connectionManager.AddSocket(socket);
    }

    public async Task Disconnect(Guid webSocketId, WebSocket webSocket,WebSocketCloseStatus closeStatus, string closeStatusDescription)
    {
        await webSocket.CloseAsync(closeStatus, closeStatusDescription, CancellationToken.None);
        _connectionManager.RemoveSocket(webSocketId);
    }

    public async Task SendMessageAsync<T>(Guid socketId, T message)
    {
        var socket = _connectionManager.GetSocketById(socketId);

        if (socket != null)
        {
            await SendMessageAsync(socket.WebSocket, message);
        }
    }

    public async Task SendMessageAsync<T>(WebSocket socket, T message)
    {
        if (socket.State != WebSocketState.Open)
            return;


        var data = new ArraySegment<byte>(JsonSerializerHelper.Serialize(message));

        await socket.SendAsync(buffer: data,
                               messageType: WebSocketMessageType.Text,
                               endOfMessage: true,
                               cancellationToken: CancellationToken.None);
    }
}
