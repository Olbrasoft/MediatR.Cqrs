using MediatR.Cqrs.Common;

namespace MediatR.Cqrs.EntityFrameworkCore.Tests;
public class PingBaseCommand : BaseCommand<PingBook>
{
 

    protected PingBaseCommand()
    {
    }
}
