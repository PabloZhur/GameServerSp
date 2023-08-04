namespace GameServerSP.Application.Exceptions
{
    public class SocketNotFoundException : Exception
    {
        public SocketNotFoundException(string message) : base(message) { }
    }
}
