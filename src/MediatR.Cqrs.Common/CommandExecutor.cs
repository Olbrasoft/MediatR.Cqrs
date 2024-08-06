namespace MediatR.Cqrs.Common;

public class CommandExecutor : ICommandExecutor
{
    private readonly IMediator _mediator;
    public event EventHandler<ExecuteEventArgs>? Executing;
    public event EventHandler<ExecuteEventArgs>? Executed;

    public CommandExecutor( IMediator mediator)
    {
        if (mediator is null) throw new ArgumentNullException(nameof(mediator));

        _mediator = mediator;
    }

    public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken token = default)
    {
        if (command is null) throw new CommandNullException();

        OnExecuting(command);

        var result = await _mediator.Send(command, token);

        OnExecuted(command, result);

        return result;
    }

    private void OnExecuting<TResult>(ICommand<TResult> command)
    {
        if (Executing is not null)
            Executing(this, new ExecuteEventArgs(command));
    }

    private void OnExecuted<TResult>(ICommand<TResult> command, TResult result)
    {
        if (Executed is not null)
            Executed(this, new ExecuteEventArgs(command, result));
    }
}