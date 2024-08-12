
namespace MediatR.Cqrs.FreeSql;

public abstract class DbQueryHandler<TContext, TEntity, TQuery, TResult> : DbRequestHandler<TContext, TEntity, TQuery, TResult>
   where TEntity : class where TContext : DbContext where TQuery : IQuery<TResult>
{
    protected DbQueryHandler(TContext context) : base(context)
    {
    }

    protected DbQueryHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }

    protected static void ThrowIfQueryIsNullOrCancellationRequested(TQuery query, CancellationToken token)
    {
        if (query is not null) token.ThrowIfCancellationRequested();
        else
            throw new ArgumentNullException(nameof(query));
    }
}


public abstract class DbQueryHandler<TContext, TEntity, TQuery>(TContext context) : DbQueryHandler<TContext, TEntity, TQuery, bool>(context)
  where TEntity : class where TContext : DbContext where TQuery : IQuery<bool>
{
}