//using GameServerSP.Application.Middlewares;
//using GameServerSP.Infrastructure.WebSocketManagement.Connections;
//using GameServerSP.Infrastructure.WebSocketManagement.Handlers;
//using System.Reflection;

//namespace GameServerSP.Application.Extentions;

//public static class WebSocketExtentions
//{
//    public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
//    {
//        services.AddSingleton<IConnectionManager, ConnectionManager>();

//        var assembly = Assembly.GetAssembly(typeof(WebSocketHandler));

//        if(assembly == null)
//        {
//            throw new Exception();
//        }

//        foreach (var type in assembly.ExportedTypes)
//        {
//            if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
//            {
//                //services.AddSingleton(type);
//            }
//        }

//        return services;
//    }

//    public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app, PathString path, WebSocketHandler handler)
//    {
//        return app.Map(path, (_app) => _app.UseMiddleware<WebSocketMiddleware>(handler));
//    }
//}
