namespace MediatR.Cqrs.Common;

/// <summary>
/// Represents a base class for queries in the MediatR.Cqrs.Common namespace.
/// </summary>
public class BaseQuery<TResult> : BaseRequest<TResult>, IQuery<TResult>
{
    /// <summary>
    /// Gets the query processor.
    /// </summary>
    public IQueryProcessor? Processor { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseQuery{TResult}"/> class with the specified query processor.
    /// </summary>
    /// <param name="processor">The query processor.</param>
    public BaseQuery(IQueryProcessor processor)
    {
        if (processor is null) throw new QueryProcessorNullException();

        Processor = processor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseQuery{TResult}"/> class with the specified mediator.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public BaseQuery(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseQuery{TResult}"/> class.
    /// </summary>
    protected BaseQuery() { }
}
