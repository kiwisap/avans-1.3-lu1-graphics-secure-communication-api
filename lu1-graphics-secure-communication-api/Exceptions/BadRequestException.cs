namespace lu1_graphics_secure_communication_api.Exceptions;

public sealed class BadRequestException : AppException
{
    public BadRequestException(string message) : base(message) { }
}