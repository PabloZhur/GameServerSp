using GameServerSP.Application.Middlewares;

namespace GameServerSP.Application.Extentions;

public static class WebSocketConnectionsMiddlewareExtensions
{
    public static IApplicationBuilder MapWebSocketConnections(this IApplicationBuilder app, PathString pathMatch)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        return app.Map(pathMatch, buidler => buidler.UseMiddleware<WebSocketMiddleware>());
    }
}
