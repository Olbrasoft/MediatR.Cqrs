namespace MediatR.Cqrs.Common;
public abstract class BaseRequest<TResult> : IRequest<TResult>
{
    public IMediator? Mediator { get; }


    protected BaseRequest(IMediator mediator)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected BaseRequest()
    {
    }
}

