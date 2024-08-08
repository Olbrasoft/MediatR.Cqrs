namespace MediatR.Cqrs.Common;

/// <summary>
/// Executes commands by sending them to the mediator.
/// </summary>
public class CommandExecutor : ICommandExecutor
{
    private readonly IMediator _mediator;
    public event EventHandler<ExecuteEventArgs>? Executing;
    public event EventHandler<ExecuteEventArgs>? Executed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandExecutor"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    public CommandExecutor(IMediator mediator)
    {
        if (mediator is not null) _mediator = mediator;
        else
            throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Executes the specified command asynchronously.
    /// </summary>
    /// <typeparam name="TResult">The type of the command result.</typeparam>
    /// <param name="command">The command to execute.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The result of the command execution.</returns>
    public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken token = default)
    {
        if (command is null) throw new CommandNullException();

        OnExecuting(command);

        var result = await _mediator.Send(command, token);

        OnExecuted(command, result);

        return result;
    }

    /// <summary>
    /// Raises the Executing event.
    /// </summary>
    /// <typeparam name="TResult">The type of the command result.</typeparam>
    /// <param name="command">The command being executed.</param>
    private void OnExecuting<TResult>(ICommand<TResult> command)
    {
        if (Executing is not null)
            Executing(this, new ExecuteEventArgs(command));
    }

    /// <summary>
    /// Raises the Executed event.
    /// </summary>
    /// <typeparam name="TResult">The type of the command result.</typeparam>
    /// <param name="command">The command that was executed.</param>
    /// <param name="result">The result of the command execution.</param>
    private void OnExecuted<TResult>(ICommand<TResult> command, TResult result)
    {
        if (Executed is not null)
            Executed(this, new ExecuteEventArgs(command, result));
    }
}
