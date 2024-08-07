using MediatR.Cqrs.Common;
using Microsoft.EntityFrameworkCore;
using Olbrasoft.Mapping;


namespace MediatR.Cqrs.EntityFrameworkCore.Tests;
public class PingDbCommandHandler : DbCommandHandler<DbContext, PingBook, BaseCommand<string>, string> 
{


    public PingDbCommandHandler(DbContext context) : base(context)
    {
    }

    public PingDbCommandHandler(IProjector projector, DbContext context) : base(projector, context)
    {
    }

    public PingDbCommandHandler(IMapper mapper, DbContext context) : base(mapper, context)
    {
    }

    public PingDbCommandHandler(IProjector projector, IMapper mapper, DbContext context) : base(projector, mapper, context)
    {
    }

    public override Task<string> Handle(BaseCommand<string> request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public static void CallThrowIfCommandIsNullOrCancellationRequested(BaseCommand<string> command, CancellationToken token)
    {
        ThrowIfCommandIsNullOrCancellationRequested(command,token);
    }

}

