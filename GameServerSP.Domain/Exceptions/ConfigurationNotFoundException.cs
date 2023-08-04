
namespace GameServerSP.Domain.Exceptions;

public class ConfigurationNotFoundException : Exception
{
    public ConfigurationNotFoundException(string message)
    : base(message) { }

    public ConfigurationNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
