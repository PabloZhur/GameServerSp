namespace GameClientServerSP.Shared.Requests;

public record UpdateResourceRequest : BaseRequest
{
    public int Coins { get; set; }
    public int Rolls { get; set; }
}
