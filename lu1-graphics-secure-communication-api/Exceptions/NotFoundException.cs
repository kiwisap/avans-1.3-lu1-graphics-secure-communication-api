namespace lu1_graphics_secure_communication_api.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message) { }
}