using MediatR.Cqrs.EntityFrameworkCore.Tests;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MediatR.Cqrs.EntityFrameworkCore;
public abstract class DbBaseCommandHandler<TContext, TEntity, TCommand, TResult> : DbCommandHandler<TContext, TEntity, TCommand, TResult>,ICommandHandler<TCommand, TResult>
   where TContext : DbContext where TEntity : class where TCommand : BaseCommand<TResult>
{

    
    protected TCommand? Command;


    protected DbBaseCommandHandler(TContext context) : base(context)
    {
    }

    protected DbBaseCommandHandler(IProjector projector, TContext context) : base(projector, context)
    {
    }

    protected DbBaseCommandHandler(IMapper mapper, TContext context) : base(mapper, context)
    {
    }

    protected DbBaseCommandHandler(IProjector projector, IMapper mapper, TContext context) : base(projector, mapper, context)
    {
    }

    

    protected void ThrowIfCommandStatusCannotBeSet(CommandStatus status)
    {
        if (TrySetCommandStatus(status))
            return;
        throw new InvalidOperationException($"Failed to set command status: {status}");
    }

    protected bool TrySetCommandStatus(CommandStatus status)
    {
        if (Command is null) return false;

        Command.Status = status;

        return true;
    }


    protected virtual async Task<CommandStatus> RemoveAndSaveAsync(Expression<Func<TEntity, bool>> exp, CancellationToken token = default)
    {
        var entity = await GetOneOrNullAsync(exp, token);

        if (entity is not null) return await RemoveAndSaveAsync(entity, token);

        TrySetCommandStatus(CommandStatus.NotFound);

        return CommandStatus.NotFound;
    }

    protected virtual async Task<CommandStatus> RemoveAndSaveAsync(TEntity detachedOrUnchangedEntity, CancellationToken token = default)
    {
        var state = GetEntityState(detachedOrUnchangedEntity);

        if (state == EntityState.Detached || state == EntityState.Unchanged)
        {
            Remove(detachedOrUnchangedEntity);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Deleted);
                return CommandStatus.Deleted;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected virtual async Task<CommandStatus> AddAndSaveAsync(TEntity detachedEntity, CancellationToken token = default)
    {
        if (GetEntityState(detachedEntity) == EntityState.Detached)
        {
            await AddAsync(detachedEntity, token);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Created);
                return CommandStatus.Created;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }

    protected virtual async Task<CommandStatus> UpdateAndSaveAsync(TEntity unchangedEntity, CancellationToken token = default)
    {
        Entities.Update(unchangedEntity);

        TrySetCommandStatus(CommandStatus.Modified);

        return await SaveAsync(unchangedEntity, token);
    }

    protected virtual async Task<CommandStatus> SaveAsync(TEntity modifiedEntity, CancellationToken token = default)
    {
        var state = GetEntityState(modifiedEntity);

        if (state == EntityState.Unchanged)
        {
            TrySetCommandStatus(CommandStatus.Unchanged);
            return CommandStatus.Unchanged;
        }

        if (state == EntityState.Modified)
        {
            TrySetCommandStatus(CommandStatus.Modified);

            if (await SaveOneEntityAsync(token))
            {
                TrySetCommandStatus(CommandStatus.Success);
                return CommandStatus.Success;
            }
        }

        TrySetCommandStatus(CommandStatus.Error);
        return CommandStatus.Error;
    }
     

    protected virtual EntityEntry<TEntity> Remove(TEntity entity)
    {
        var result = Entities.Remove(entity);
        TrySetCommandStatus(CommandStatus.Removed);
        return result;
    }

    protected virtual async ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await Entities.AddAsync(entity, cancellationToken);

        TrySetCommandStatus(CommandStatus.Added);

        return result;
    }

    protected virtual EntityEntry<TEntity> Update(TEntity entity)
    {
        var result = Entities.Update(entity);
        TrySetCommandStatus(CommandStatus.Modified);
        return result;
    }



}

