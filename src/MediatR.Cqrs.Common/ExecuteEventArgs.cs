namespace MediatR.Cqrs.Common;

/// <summary>
/// Represents the event arguments for the execution of a command.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExecuteEventArgs"/> class.
/// </remarks>
/// <param name="command">The command being executed.</param>
/// <param name="result">The result of the execution.</param>
public class ExecuteEventArgs(object command, object? result = null) : EventArgs
{
    /// <summary>
    /// Gets the command being executed.
    /// </summary>
    public object Command { get; } = command;

    /// <summary>
    /// Gets the result of the execution.
    /// </summary>
    public object? Result { get; } = result;
}
