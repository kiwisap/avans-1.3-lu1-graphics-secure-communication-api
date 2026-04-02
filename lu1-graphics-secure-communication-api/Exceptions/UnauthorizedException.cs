namespace lu1_graphics_secure_communication_api.Exceptions;

public sealed class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message) { }
}