namespace MediatR.Cqrs.EntityFrameworkCore;
public abstract class DbRequestHandler<TContext, TEntity, TRequest, TResult>(TContext context) : IRequestHandler<TRequest, TResult>
    where TEntity : class where TContext : DbContext where TRequest : IRequest<TResult>
{   
    private DbSet<TEntity>? _entities;
    private readonly IProjector? _projector;

    protected virtual TContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context));
    protected DbSet<TEntity> Entities { get => _entities is null ? Context.Set<TEntity>() : _entities; private set => _entities = value; }

    public DbRequestHandler(IProjector projector, TContext context) : this(context)
    => _projector = projector ?? throw new ArgumentNullException(nameof(projector));

    /// <summary>
    /// Query conditions，Where(a => a.Id > 10)，Support navigation object query，Where(a => a.Author.Email == "2881099@qq.com")
    /// </summary>
    /// <param name="expression">lambda expression</param>
    /// <returns>Queryable where</returns>
    protected IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> expression) => Entities.Where(expression);

    /// <summary>
    /// Sort by column ascending，OrderBy(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by ascending</typeparam>
    /// <param name="column">Selector property/column for ascending order</param>
    /// <returns>Ascending selection</returns>
    protected IOrderedQueryable<TEntity> GetOrderBy<TMember>(Expression<Func<TEntity, TMember>> columnSelector) => Entities.OrderBy(columnSelector);

    /// <summary>
    /// Sort by column descending，OrderByDescending(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by descending</typeparam>
    /// <param name="columnSelector">Selector property/column for descending order</param>
    /// <returns>Descending selection</returns>
    protected IOrderedQueryable<TEntity> GetOrderByDescending<TMember>(Expression<Func<TEntity, TMember>> columnSelector)
        => Entities.OrderByDescending(columnSelector);

    #region ExistsAsync

    protected Task<bool> ExistsAsync(CancellationToken token = default) => Entities.AnyAsync(token);

    protected async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        => await Entities.AnyAsync(predicate, token);

    #endregion

    #region GetEnumerableAsync

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
       where TDestination : new()
      => await ProjectTo<TDestination>(Entities).ToArrayAsync(token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(IQueryable queryable, CancellationToken token)
     where TDestination : new()
     => await ProjectTo<TDestination>(queryable).ToArrayAsync(token);

    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(IQueryable<TEntity> queryable, CancellationToken token)
     => await queryable.ToArrayAsync(token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> expression, CancellationToken token)
     where TDestination : new()
      => await ProjectTo<TDestination>(GetWhere(expression)).ToArrayAsync(token);

    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token)
     => await GetWhere(exp).ToArrayAsync(token);

    #endregion

    #region GetOneOrNullAsync

    protected async Task<TEntity?> GetOneOrNullAsync(IQueryable<TEntity> queryable, CancellationToken token)
          => await queryable.SingleOrDefaultAsync(token);

    protected async Task<TDestination?> GetOneOrNullAsync<TDestination>(IQueryable<TEntity> queryable, CancellationToken token) where TDestination : new()
          => await ProjectTo<TDestination>(queryable).SingleOrDefaultAsync(token);

    protected async Task<TEntity?> GetOneOrNullAsync(Expression<Func<TEntity, bool>> expression, CancellationToken token)
    => await GetOneOrNullAsync(GetWhere(expression), token);

    protected async Task<TDestination?> GetOneOrNullAsync<TDestination>(Expression<Func<TEntity, bool>> expression, CancellationToken token)
    where TDestination : new()
         => await GetOneOrNullAsync<TDestination>(GetWhere(expression), token);

    #endregion

    public abstract Task<TResult> Handle(TRequest request, CancellationToken token);

    protected IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
       => _projector is null ? throw new NullReferenceException(nameof(_projector)) : _projector.ProjectTo<TDestination>(source);
}
