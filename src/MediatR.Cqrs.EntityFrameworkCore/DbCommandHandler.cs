namespace MediatR.Cqrs.EntityFrameworkCore.Tests;


public abstract class DbCommandHandler<TContext, TEntity, TCommand, TResult> : DbRequestHandler<TContext, TEntity, TCommand, TResult>
    where TContext : DbContext
    where TEntity : class
    where TCommand : ICommand<TResult>
{

    protected virtual IMapper? Mapper { get; }

    protected DbCommandHandler(TContext context) : base(context)
    {
    }

    protected DbCommandHandler(IProjector projector, TContext context) : base(projector, context)
    {
    }


    protected DbCommandHandler(IMapper mapper, TContext context) : this(context)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public DbCommandHandler(IProjector projector, IMapper mapper, TContext context) : this(projector, context)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    protected virtual async Task<bool> SaveOneEntityAsync(CancellationToken token = default)
    {
        return await Context.SaveChangesAsync(token) == 1;
    }

    protected virtual Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        return Context.SaveChangesAsync(token);
    }

    protected virtual EntityState GetEntityState(object entity) => Context.Entry(entity).State;


    protected static void ThrowIfCommandIsNullOrCancellationRequested(TCommand command, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (command is null)
            throw new ArgumentNullException(nameof(command));
    }


    /// <summary>
    /// Execute a mapping from the command to a new entity.
    /// The source type is inferred from the source object.
    /// </summary>
    /// <param name="command">TCommand to map from</param>
    /// <returns>Mapped entity</returns>
    protected TEntity MapCommandToNewEntity(TCommand command)
        => Mapper is null ? throw new NullReferenceException(nameof(Mapper)) : Mapper.MapSourceToNewDestination<TEntity>(command);

    /// <summary>
    /// Execute a mapping from the command to the existing entity.
    /// </summary>
    /// <param name="command">Command object to map from</param>
    /// <param name="entity">Destination object to map into</param>
    /// <returns>The mapped destination object, same instance as the <paramref name="entity"/> object and returns.</returns>
    protected TEntity MapCommandToExistingEntity(TCommand command, TEntity entity)
    {
        if (Mapper is null) throw new NullReferenceException(nameof(Mapper));
        Mapper.MapSourceToExistingDestination(command, entity);
        return entity;
    }

    protected TDestination MapTo<TDestination>(object source)
    {
        if (Mapper is null) throw new NullReferenceException(nameof(Mapper));
        return Mapper.MapTo<TDestination>(source);
    }

}



