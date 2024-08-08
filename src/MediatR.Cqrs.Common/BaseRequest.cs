namespace MediatR.Cqrs.Common;

/// <summary>
/// Base class for requests in MediatR CQRS pattern.
/// </summary>
public abstract class BaseRequest<TResult> : IRequest<TResult>
{
    public IMediator? Mediator { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRequest{TResult}"/> class with a mediator.
    /// </summary>
    /// <param name="mediator">The mediator instance.</param>
    protected BaseRequest(IMediator mediator)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected BaseRequest()
    {
    }
}

