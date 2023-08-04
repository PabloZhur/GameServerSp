using GameClientServerSP.Shared.Responses;
using GameClientServerSP.Shared;
using System.Net.WebSockets;
using GameClientServerSP.Shared.Requests;
using Serilog;
using System;

namespace GameClientSP;

public class CustomWebSocketClient
{
    private readonly string _url;
    private ClientWebSocket _wsClient;
    private readonly ILogger _logger;
    public CustomWebSocketClient(string url, ILogger logger)
    {
        _logger = logger;
        _url = url;
    }
    public async Task<ClientWebSocket> ConnectToWebSocket()
    {
        _logger.Information("Connection to server");

        _wsClient = new ClientWebSocket();

        await _wsClient.ConnectAsync(new Uri(_url), CancellationToken.None);

        _logger.Information("Connected!");

        return _wsClient;
    }

    public async Task SendMessages()
    {
        await Task.Delay(100);
        while (true)
        {
            Console.WriteLine("---Type 0 to login");
            Console.WriteLine("---Type 1 to updateResource");
            Console.WriteLine("---Type 2 to sendGift");
            Console.WriteLine("---Type \"exit\" to close connection");

            var command = Console.ReadLine();

            if (command == "0")
            {
                var guids = PredifiendGuids.GetGuids();
                for (int i = 0; i < guids.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {guids[i]}");
                }
                Console.WriteLine();

                var deviceNumber = GetValue($"Choose the device number: < {guids.Count}");

                Guid guid;
                if(deviceNumber >= 0 && deviceNumber < guids.Count)
                {
                    guid = guids[deviceNumber-1];
                }
                else
                {
                    Console.WriteLine("Device number is out of range. Random guid will be generated");
                    guid = Guid.NewGuid();
                }

                var loginRequest = new LoginRequest { DeviceId = guid };

                var loginMessage = new ArraySegment<byte>(JsonSerializerHelper.Serialize(loginRequest));
                await _wsClient.SendAsync(loginMessage, WebSocketMessageType.Text, true, CancellationToken.None);
                _logger.Information("Log message was sent");
            }
            else if (command == "1")
            {
                int coins = GetValue("Type Coins amount");
                int rolls = GetValue("Type Roll amount");
         
                var updateResourceRequest = new UpdateResourceRequest { Coins = coins, Rolls = rolls };
                var updateResourceMessage = new ArraySegment<byte>(JsonSerializerHelper.Serialize(updateResourceRequest));
                await _wsClient.SendAsync(updateResourceMessage, WebSocketMessageType.Text, true, CancellationToken.None);
                _logger.Information("Update resource message was sent");
            }
            else if (command == "2")
            {
                var playerId = GetValue("Type Player Id");
                var giftType = GetResourceType();
                var giftValue = GetValue("Type gift value");

                var sendGift = new GiftRequest { FriendPlayerId = playerId, Type = giftType, Value = giftValue };
                var sendGiftMessage = new ArraySegment<byte>(JsonSerializerHelper.Serialize(sendGift));
                await _wsClient.SendAsync(sendGiftMessage, WebSocketMessageType.Text, true, CancellationToken.None);
                _logger.Information("Send Gift message was sent");
            }
            else if (command == "exit")
            {
                break;
            }
            await Task.Delay(100);
        }
    }

    public async Task ReceiveMessage()
    {
        var buffer = new byte[4096];

        while (true)
        {
            var result = await _wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                break;
            }

            var baseResponse = JsonSerializerHelper.Deserialize<BaseResponse>(buffer);
            buffer = new byte[4096];

            if (baseResponse.Success)
            {
                if (baseResponse is LoginResponse loginResponse)
                {
                    _logger.Information("Received from login - PlayerId: " + loginResponse.PlayerId);
                }
                else if (baseResponse is UpdateResourceResponse updateResourceResponse)
                {
                    _logger.Information($"Received from updateResource - Player {updateResourceResponse.PlayerId} with new balance coins: {updateResourceResponse.Coins}, rolls: {updateResourceResponse.Rolls}");
                }
                else if (baseResponse is SendGiftEvent sendGiftEvent)
                {
                    _logger.Information($"Recieved from sendGift - You got gift {sendGiftEvent.Type} {sendGiftEvent.Value} from {sendGiftEvent.PlayerId}");
                }
            }
            else
            {
                _logger.Warning("Errors: " + string.Join(";", baseResponse.Errors));
            }
        }
    }

    public async Task TryClose()
    {
        if (_wsClient.State != WebSocketState.Closed)
        {
            await _wsClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
    }

    private ResourceType GetResourceType()
    {
        while (true)
        {
            Console.WriteLine("Specify Resource Type: Coins, Rolls");
            var typeStr = Console.ReadLine();
            if (Enum.TryParse(typeStr, out ResourceType resourceType))
            {
                return resourceType;
            }

            Console.WriteLine("Incorrect value. Try again");
        }
    }

    private int GetValue(string message)
    {
        while (true)
        {
            Console.WriteLine(message);
            var coinsString = Console.ReadLine();

            if (int.TryParse(coinsString, out var value))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Incorrect value. Try again");
            }
        }

    }
}
