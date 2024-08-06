namespace MediatR.Cqrs.Common;

public class QueryProcessor : IQueryProcessor
{
    private readonly IMediator _mediator;
    public event EventHandler<ProcessEventArgs>? Processing;
    public event EventHandler<ProcessEventArgs>? Processed;

    public QueryProcessor(IMediator mediator)
    {
        if (mediator is not null)
        {
            _mediator = mediator;
        }
        else
        {
            throw new ArgumentNullException(nameof(mediator));
        }
    }

    public async Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, CancellationToken token = default)
    {
        if (query is null) throw new QueryNullException();

        OnProcessing(query);

        var result = await _mediator.Send(query, token);

        OnProcessed(query, result);

        return result;
    }

    private void OnProcessing<TResult>(IQuery<TResult> query)
    {
        if (Processing is not null)
            Processing(this, new ProcessEventArgs(query));
    }

    private void OnProcessed<TResult>(IQuery<TResult> query, TResult result)
    {
        if (Processed is not null)
            Processed(this, new ProcessEventArgs(query, result));
    }

  
}