using AutoMapper;
using GameServerSP.Application.Options;
using GameServerSP.Application.Services.WebSocketManagement;
using GameServerSP.Application.Services.WebSocketManagement.Handlers;
using MediatR;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;

namespace GameServerSP.Application.Middlewares;

public class WebSocketMiddleware
{
    private readonly WebSocketConnectionOptions _options;
    private readonly RequestDelegate _next;
    private readonly ILogger<WebSocketMiddleware> _logger;
    public WebSocketMiddleware(
        IOptions<WebSocketConnectionOptions> options,
        RequestDelegate next,
        ILogger<WebSocketMiddleware> logger)
    {
        _options = options.Value;
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IWebSocketHandler webSocketHandler, IMediator mediator, IMapper mapper)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var webSocketId = webSocketHandler.Connect(webSocket);
            _logger.LogInformation($"Web socket connection was accepted with id = {webSocketId}");

            var webSocketConnection = new WebSocketConnection(webSocketId, webSocket, _options.BufferSize, mediator, mapper);

            try
            {
                await webSocketConnection.HandleMessagesUntilCloseAsync();

                if (webSocketConnection.CloseStatus.HasValue)
                {
                    await webSocketHandler.Disconnect(webSocketId, webSocket, webSocketConnection.CloseStatus.Value, webSocketConnection.CloseStatusDescription);
                    _logger.LogWarning($"Web socket connectio with id = {webSocketId} was closed");
                }
            }
            catch (WebSocketException wsex) when (wsex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                webSocketHandler.ConnectionManager.RemoveSocket(webSocketId);
                _logger.LogWarning($"Web socket connectio with id = {webSocketId} was aborted");

            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Error: it's not web socket connection");
        }
    }
}

