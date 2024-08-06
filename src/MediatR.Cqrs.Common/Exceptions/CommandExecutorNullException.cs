namespace MediatR.Cqrs.Common.Exceptions;

public class CommandExecutorNullException : ArgumentNullException
{
    public CommandExecutorNullException() : base("executor")
    {

    }
}