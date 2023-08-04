namespace GameClientServerSP.Shared.Requests;

public record LoginRequest : BaseRequest
{
    public Guid DeviceId { get; set; }
}
