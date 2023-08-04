namespace GameClientServerSP.Shared.Responses;

public class BaseResponse
{
    public List<string> Errors { get; set; } = new List<string>();
    public bool Success => !Errors.Any();
}
