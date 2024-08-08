﻿namespace MediatR.Cqrs.EntityFrameworkCore;


/// <summary>
/// Base class for database command handlers.
/// </summary>
/// <typeparam name="TContext">The type of the database context.</typeparam>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TCommand">The type of the command.</typeparam>
/// <typeparam name="TResult">The type of the result.</typeparam>
public abstract class DbCommandHandler<TContext, TEntity, TCommand, TResult> : DbRequestHandler<TContext, TEntity, TCommand, TResult>
    where TContext : DbContext
    where TEntity : class
    where TCommand : ICommand<TResult>
{
    protected virtual IMapper? Mapper { get; }


    /// <summary>
    /// Initializes a new instance of the <see cref="DbCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    protected DbCommandHandler(TContext context) : base(context)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="projector">The projector.</param>
    /// <param name="context">The database context.</param>
    protected DbCommandHandler(IProjector projector, TContext context) : base(projector, context)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="context"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected DbCommandHandler(IMapper mapper, TContext context) : this(context)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbCommandHandler{TContext, TEntity, TCommand, TResult}"/> class.
    /// </summary>
    /// <param name="projector">The projector.</param>
    /// <param name="mapper">The mapper.</param>
    /// <param name="context">The database context.</param>
    protected DbCommandHandler(IProjector projector, IMapper mapper, TContext context) : this(projector, context)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Saves a single entity asynchronously.
    /// </summary>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result indicates whether the save operation was successful (true) or not (false).</returns>
    protected virtual async Task<bool> SaveOneEntityAsync(CancellationToken token = default)
    {
        return await Context.SaveChangesAsync(token) == 1;
    }

    /// <summary>
    /// Saves all changes made in this context to the underlying database asynchronously.
    /// </summary>
    /// <param name="token">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    protected virtual Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        return Context.SaveChangesAsync(token);
    }

    /// <summary>
    /// Gets the entity state of the specified entity.
    /// </summary>
    /// <param name="entity">The entity object.</param>
    /// <returns>The entity state.</returns>
    protected virtual EntityState GetEntityState(object entity) => Context.Entry(entity).State;


    /// <summary>
    /// Throws an exception if the command is null or if cancellation is requested.
    /// </summary>
    /// <param name="command">The command object.</param>
    /// <param name="token">The cancellation token.</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
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

    /// <summary>
    /// Execute a mapping from the source object to the destination object of type TDestination.
    /// </summary>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="source">The source object to map from.</param>
    /// <returns>The mapped destination object of type TDestination.</returns>
    /// <exception cref="NullReferenceException">Thrown when the Mapper is null.</exception>
    protected TDestination MapTo<TDestination>(object source)
    {
        if (Mapper is null) throw new NullReferenceException(nameof(Mapper));
        return Mapper.MapTo<TDestination>(source);
    }

}



