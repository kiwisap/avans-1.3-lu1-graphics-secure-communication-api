namespace lu1_graphics_secure_communication_api.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }
}