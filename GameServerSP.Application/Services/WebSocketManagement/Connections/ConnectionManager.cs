using GameServerSP.Application.Exceptions;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace GameServerSP.Application.Services.WebSocketManagement.Connections;

public class ConnectionManager : IConnectionManager
{
    private ConcurrentDictionary<Guid, AuthenticatedWebSocket> _sockets = new ConcurrentDictionary<Guid, AuthenticatedWebSocket>();

    private ConcurrentDictionary<int, Guid> _playerSocketMapping = new ConcurrentDictionary<int, Guid>();

    public ConcurrentDictionary<Guid, AuthenticatedWebSocket> GetAll()
    {
        return _sockets;
    }

    public Guid AddSocket(WebSocket socket)
    {
        var guid = Guid.NewGuid();
        _sockets.TryAdd(guid, new AuthenticatedWebSocket(socket));

        return guid;
    }

    public void AuthenticateSocket(Guid socketId, int playerId)
    {
        var socket = GetSocketById(socketId);
        socket.IsAuthenticated = true;
        socket.PlayerId = playerId;
        _playerSocketMapping[playerId] = socketId;
    }

    public bool IsSocketAuthenticated(Guid socketId)
    {
        var socket = GetSocketById(socketId);
        return socket.IsAuthenticated;
    }

    public bool IsPlayerAuthenticated(int playerId)
    {
        return _playerSocketMapping.ContainsKey(playerId);
    }

    public async void RemoveSocket(Guid id)
    {
        foreach(var key in _playerSocketMapping.Keys)
        {
            if (_playerSocketMapping[key] == id)
            {
                _playerSocketMapping.Remove(key, out var _);
                break;
            }
        };
       
        if (_sockets.TryRemove(id, out var socket))
        {
        }
        else
        {
            throw new SocketNotFoundException($"Socket with {id} was not found");
        }
    }

    public AuthenticatedWebSocket GetSocketById(Guid id)
    {
        if (_sockets.TryGetValue(id, out var socket))
        {
            return socket;
        }

        throw new SocketNotFoundException($"Socket with {id} was not found");
    }

    public bool TryGetWebSocketByPlayerId(int playerId, out AuthenticatedWebSocket socket)
    {
        socket = null;
        if (_playerSocketMapping.TryGetValue(playerId, out var socketId))
        {
            if(_sockets.TryGetValue(socketId, out socket))
            {
                return true;
            }
        }

        return false;
    }
}

public class AuthenticatedWebSocket
{
    public AuthenticatedWebSocket(WebSocket webSocket)
    {
        WebSocket = webSocket;
    }

    public WebSocket WebSocket { get; set; }
    public bool IsAuthenticated { get; set; }
    public int PlayerId { get; set; }
}