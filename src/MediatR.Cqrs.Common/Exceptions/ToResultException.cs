namespace MediatR.Cqrs.Common.Exceptions;

public class ToResultException : InvalidOperationException
{
    public ToResultException(string paramName) : base($"{paramName} and Mediator is null.")
    {
    }
}