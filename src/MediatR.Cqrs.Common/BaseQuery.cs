namespace MediatR.Cqrs.Common;
public class BaseQuery<TResult> : BaseRequest<TResult>, IQuery<TResult>
{
    public IQueryProcessor? Processor { get; }

    public BaseQuery(IQueryProcessor processor)
    {
        if (processor is null) throw new QueryProcessorNullException();

        Processor = processor;
    }

    public BaseQuery(IMediator mediator) : base(mediator)
    {
    }

    protected BaseQuery()
    {
    }
}
