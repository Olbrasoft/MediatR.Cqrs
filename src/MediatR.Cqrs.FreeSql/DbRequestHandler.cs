namespace MediatR.Cqrs.FreeSql;
public abstract class DbRequestHandler<TContext, TEntity, TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TEntity : class where TContext : DbContext where TRequest : IRequest<TResult>
{
    protected IConfigure<TEntity>? ProjectionConfigurator { get; }

    private ISelect<TEntity>? _select;

    protected IMapper? Mapper { get; }

    protected virtual TContext Context { get; }

    protected ISelect<TEntity> Select
    {
        get => _select is not null ? _select : GetSelect();

        set => _select = value;
    }


    protected DbRequestHandler(TContext context)
        => Context = context ?? throw new ArgumentNullException(nameof(context));

    protected DbRequestHandler(IConfigure<TEntity> projectionConfigurator, TContext context) : this(context)
       => ProjectionConfigurator = projectionConfigurator ?? throw new ArgumentNullException(nameof(projectionConfigurator));

    public DbRequestHandler(IMapper mapper, TContext context) : this(context)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    protected Expression<Func<TEntity, TDestination>> ProjectionConfigure<TDestination>() where TDestination : new()
        => ProjectionConfigurator is null
            ? throw new NullReferenceException($"{nameof(ProjectionConfigurator)} is null !")
            : ProjectionConfigurator.Configure<TDestination>();

    public abstract Task<TResult> Handle(TRequest request, CancellationToken token);

    /// <summary>
    /// Represents the select keyword from Structured Query Language.
    /// </summary>
    /// <returns>Select</returns>
    private ISelect<TEntity> GetSelect() => Context.Set<TEntity>().Select;


    protected ISelect<TForeignEntity> GetSelect<TForeignEntity>() where TForeignEntity : class => Context.Set<TForeignEntity>().Select;

    /// <summary>
    /// Query conditions，Where(a => a.Id > 10)，Support navigation object query，Where(a => a.Author.Email == "2881099@qq.com")
    /// </summary>
    /// <param name="condition">lambda expression</param>
    /// <returns>Select where</returns>
    protected ISelect<TEntity> GetWhere(Expression<Func<TEntity, bool>> condition) => Select.Where(condition);

    /// <summary>
    /// Sort by column ascending，OrderBy(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by ascending</typeparam>
    /// <param name="column">Selector property/column for ascending order</param>
    /// <returns>Ascending selection</returns>
    protected ISelect<TEntity> GetOrderBy<TMember>(Expression<Func<TEntity, TMember>> columnSelector) => Select.OrderBy(columnSelector);

    /// <summary>
    /// Sort by column descending，OrderByDescending(a => a.Time)
    /// </summary>
    /// <typeparam name="TMember">property/column to sort by descending</typeparam>
    /// <param name="columnSelector">Selector property/column for descending order</param>
    /// <returns>Descending selection</returns>
    protected ISelect<TEntity> GetOrderByDescending<TMember>(Expression<Func<TEntity, TMember>> columnSelector)
        => Select.OrderByDescending(columnSelector);

    protected Task<bool> ExistsAsync(CancellationToken token = default) => Select.AnyAsync(token);


    protected async Task<TEntity> GetOneOrNullAsync(ISelect<TEntity> select, CancellationToken token)
      => await select.ToOneAsync(token);

    protected async Task<TDestination> GetOneOrNullAsync<TDestination>(ISelect<TEntity> select, CancellationToken token) where TDestination : new()
    {
        if (ProjectionConfigurator is not null)
            return await select.ToOneAsync(ProjectionConfigure<TDestination>(), token);
        else
            return await select.ToOneAsync<TDestination>(token);
    }

    #region GetEnumerableAsync

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(CancellationToken token)
       where TDestination : new()
      => await Select.ToListAsync(ProjectionConfigure<TDestination>(), token);

    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(ISelect<TEntity> select, CancellationToken token)
      where TDestination : new()
      => await select.ToListAsync(ProjectionConfigure<TDestination>(), token);


    protected async Task<IEnumerable<TDestination>> GetEnumerableAsync<TDestination>(Expression<Func<TEntity, bool>> condition, CancellationToken token)
     where TDestination : new()
      => await GetWhere(condition).ToListAsync(ProjectionConfigure<TDestination>(), token);
   
    
    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(ISelect<TEntity> select, CancellationToken token)
      => await select.ToListAsync(token);

    protected async Task<IEnumerable<TEntity>> GetEnumerableAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token)
      => await GetWhere(condition).ToListAsync(token);

    #endregion GetEnumerableAsync

    protected async Task<TEntity> GetOneOrNullAsync(Expression<Func<TEntity, bool>> condition, CancellationToken token)
      => await GetOneOrNullAsync(GetWhere(condition), token);

    protected async Task<TDestination> GetOneOrNullAsync<TDestination>(Expression<Func<TEntity, bool>> condition, CancellationToken token)
        where TDestination : new()
      => await GetOneOrNullAsync<TDestination>(GetWhere(condition), token);
}
