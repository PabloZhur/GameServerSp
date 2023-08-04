using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace GameServerSP.Application.Services.WebSocketManagement.Connections
{
    public interface IConnectionManager
    {
        Guid AddSocket(WebSocket socket);
        void RemoveSocket(Guid id);
        ConcurrentDictionary<Guid, AuthenticatedWebSocket> GetAll();
        AuthenticatedWebSocket GetSocketById(Guid id);
        void AuthenticateSocket(Guid socketId, int playerId);
        bool IsSocketAuthenticated(Guid socketId);
        bool TryGetWebSocketByPlayerId(int playerId, out AuthenticatedWebSocket socket);
        bool IsPlayerAuthenticated(int playerId);
    }
}