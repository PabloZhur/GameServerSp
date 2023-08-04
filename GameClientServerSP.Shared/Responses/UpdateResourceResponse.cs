
namespace GameClientServerSP.Shared.Responses;

public class UpdateResourceResponse : BaseResponse
{
    public int PlayerId { get; set; }
    public int Coins { get; set; }
    public int Rolls { get; set; }
}
