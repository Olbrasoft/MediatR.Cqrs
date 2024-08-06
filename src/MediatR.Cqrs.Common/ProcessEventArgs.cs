namespace MediatR.Cqrs.Common;

public class ProcessEventArgs(object query, object? result = null) : EventArgs
{
    public object Query { get; } = query ?? throw new ArgumentNullException(nameof(query));
    public object? Result { get; } = result;
}