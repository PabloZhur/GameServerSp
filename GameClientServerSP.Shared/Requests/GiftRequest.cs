namespace GameClientServerSP.Shared.Requests;

public record GiftRequest : BaseRequest
{
    public int FriendPlayerId { get; set; }
    public ResourceType Type { get; set; }
    public int Value { get; set; }

}

public enum ResourceType
{
    Coins,
    Rolls
}
