// See https://aka.ms/new-console-template for more information
using GameClientSP;
using Serilog;

var url = $"ws://localhost:5007/gameServer";

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.File("serilog/clientLogs.txt")
        .CreateLogger();

var wsClient = new CustomWebSocketClient(url, Log.Logger);

await wsClient.ConnectToWebSocket();

var sendTask = wsClient.SendMessages();
var receiveTask = wsClient.ReceiveMessage();


await Task.WhenAny(sendTask, receiveTask);

await wsClient.TryClose();

await Task.WhenAll(sendTask, receiveTask);

