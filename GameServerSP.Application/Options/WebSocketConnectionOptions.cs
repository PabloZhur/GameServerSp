namespace GameServerSP.Application.Options;

public class WebSocketConnectionOptions
{
    public int BufferSize { get; set; }

    public WebSocketConnectionOptions()
    {
        BufferSize = 4 * 1024;
    }
}
