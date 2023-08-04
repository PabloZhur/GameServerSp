using GameClientServerSP.Shared.Requests;

namespace GameClientServerSP.Shared.Responses;

public class SendGiftEvent : BaseResponse
{
    public int PlayerId { get; set; }
    public ResourceType Type { get; set; }

    public int Value { get; set; }
}
