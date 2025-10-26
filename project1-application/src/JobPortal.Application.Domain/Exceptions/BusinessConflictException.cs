namespace JobPortal.Application.Domain.Exceptions;

public class BusinessConflictException : DomainException
{
    public BusinessConflictException(string message) : base(message)
    {
    }

    public BusinessConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
