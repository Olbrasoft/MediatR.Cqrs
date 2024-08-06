namespace MediatR.Cqrs.Common;

public static class BaseQueryExtensions
{
    public static Task<TResult> ToResultAsync<TResult>(this BaseQuery<TResult> query, CancellationToken token = default)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (query.Processor is not null) return query.Processor.ProcessAsync(query, token);

        if (query.Mediator is not null) return query.Mediator.Send(query, token);

        throw new ToResultException(nameof(query.Processor));
    }
}