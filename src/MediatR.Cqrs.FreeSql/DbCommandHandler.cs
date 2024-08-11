namespace MediatR.Cqrs.FreeSql;

public abstract class DbCommandHandler<TContext, TEntity, TCommand, TResult> : DbRequestHandler<TContext, TEntity, TCommand, TResult>
   where TEntity : class where TContext : DbContext where TCommand : ICommand<TResult>
{


    protected virtual DbSet<TEntity> Entities => Context.Set<TEntity>();


    protected DbCommandHandler(TContext context) : base(context)
    {
    }

    protected DbCommandHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }


    protected DbSet<TForeignEntity> GetSet<TForeignEntity>() where TForeignEntity : class
    {
        return Context.Set<TForeignEntity>();
    }


    protected virtual async Task<bool> SaveOneEntityAsync(CancellationToken token) => await Context.SaveChangesAsync(token) == 1;



    /// <summary>
    /// Throws an exception if the command is null or cancellation is requested.
    /// </summary>
    /// <param name="command">The command object.</param>
    /// <param name="token">The cancellation token.</param>
    protected static void ThrowIfCommandIsNullOrCancellationRequested(TCommand command, CancellationToken token)
    {
        if (command is not null)
            token.ThrowIfCancellationRequested();
        else
            throw new ArgumentNullException(nameof(command));
    }


    /// <summary>
    /// Maps the source object to the specified destination type.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="source">The source object to map from.</param>
    /// <returns>The mapped destination object.</returns>
    protected virtual TDestination MapTo<TDestination>(object source)
    {
        return GetMapper().MapTo<TDestination>(source);
    }



    /// <summary>
    /// Maps the command object to a new entity.
    /// </summary>
    /// <param name="command">The command object to map from.</param>
    /// <returns>The mapped new entity.</returns>
    protected virtual TEntity MapCommandToNewEntity(TCommand command) => GetMapper().MapSourceToNewDestination<TEntity>(command);


    /// <summary>
    /// Execute a mapping from the command to the existing entity.
    /// </summary>
    /// <param name="command">Command object to map from</param>
    /// <param name="entity">Destination object to map into</param>
    /// <returns>The mapped destination object, same instance as the <paramref name="entity"/> object and returns.</returns>
    protected virtual TEntity MapCommandToExistingEntity(TCommand command, TEntity entity)
    {
        GetMapper().MapSourceToExistingDestination(command, entity);
        return entity;
    }



    /// <summary>
    /// Gets the mapper instance.
    /// </summary>
    /// <returns>The mapper instance.</returns>
    protected virtual IMapper GetMapper() => Mapper is null ? throw new NullReferenceException(nameof(Mapper)) : Mapper;
}

