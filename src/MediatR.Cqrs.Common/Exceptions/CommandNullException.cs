namespace MediatR.Cqrs.Common.Exceptions;

public class CommandNullException : ArgumentNullException
{
    public CommandNullException() : base("command")
    {

    }

}