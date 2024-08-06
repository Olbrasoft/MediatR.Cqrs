namespace MediatR.Cqrs.Common;

public static class BaseCommandExtensions
{
    public static Task<TResult> ToResultAsync<TResult>(this BaseCommand<TResult> command, CancellationToken token = default)
    {
        if (command is null) throw new CommandNullException();

        if (command.Executor is not null) return command.Executor.ExecuteAsync(command, token);

        if (command.Mediator is not null) return command.Mediator.Send(command, token);

        throw new ToResultException(nameof(command.Executor));
    }
}