namespace GameServerSP.Application.Models;

public class ServiceResponse<T>
{
    public ServiceResponse(T result)
    {
        Result = result;
        Errors = new List<string>();
    }

    public ServiceResponse(IEnumerable<string> errors)
    {
        Errors = errors.ToList();
    }
    public ServiceResponse(string errors)
    {
        Errors = new List<string> { errors };
    }


    public T? Result { get; }

    public List<string> Errors { get; }

    public bool Success => !Errors.Any();
}
