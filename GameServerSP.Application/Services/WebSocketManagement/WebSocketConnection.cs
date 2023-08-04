using AutoMapper;
using GameClientServerSP.Shared;
using GameClientServerSP.Shared.Requests;
using GameServerSP.Application.Models.Requests;
using MediatR;
using System.Net.WebSockets;

namespace GameServerSP.Application.Services.WebSocketManagement;

public class WebSocketConnection
{
    private readonly Guid _webSocketId;
    private readonly WebSocket _webSocket;
    private readonly int _bufferSize;

    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public WebSocketCloseStatus? CloseStatus { get; private set; } = null;
    public string? CloseStatusDescription { get; private set; } = null;

    public WebSocketConnection(
        Guid webSocketId,
        WebSocket webSocket,
        int receivePayloadBufferSize,
        IMediator mediator,
        IMapper mapper)
    {
        _webSocketId = webSocketId;
        _webSocket = webSocket;
        _bufferSize = receivePayloadBufferSize;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task HandleMessagesUntilCloseAsync()
    {
        while (_webSocket.State == WebSocketState.Open)
        {
            var buffer = new byte[_bufferSize];

            WebSocketReceiveResult webSocketReceiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            var webSocketMessage = await ReceiveMessagePayloadAsync(webSocketReceiveResult, buffer);
            if (webSocketReceiveResult.MessageType == WebSocketMessageType.Text)
            {
                var request = JsonSerializerHelper.Deserialize<BaseRequest>(webSocketMessage);

                if (request != null)
                {
                    var command = (WebSocketRequest)_mapper.Map(request, request.GetType(), typeof(WebSocketRequest));
                    command.WebSocketId = _webSocketId;

                    await _mediator.Send(command);
                }

            }
            else if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
            {
                CloseStatus = webSocketReceiveResult.CloseStatus;
                CloseStatusDescription = webSocketReceiveResult.CloseStatusDescription;
                break;
            }
        }
    }

    private async Task<byte[]> ReceiveMessagePayloadAsync(WebSocketReceiveResult webSocketReceiveResult, byte[] buffer)
    {
        byte[] messagePayload = null;

        if (webSocketReceiveResult.EndOfMessage)
        {
            messagePayload = new byte[webSocketReceiveResult.Count];
            Array.Copy(buffer, messagePayload, webSocketReceiveResult.Count);
        }
        else
        {
            using (MemoryStream messagePayloadStream = new MemoryStream())
            {
                messagePayloadStream.Write(buffer, 0, webSocketReceiveResult.Count);
                while (!webSocketReceiveResult.EndOfMessage)
                {
                    webSocketReceiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    messagePayloadStream.Write(buffer, 0, webSocketReceiveResult.Count);
                }

                messagePayload = messagePayloadStream.ToArray();
            }
        }

        return messagePayload;
    }
}

