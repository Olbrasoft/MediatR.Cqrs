using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.Cqrs.FreeSql.Tests;
public class PingBookToPingBookDtoConfigurator : IConfigure<PingBook>
{
    public System.Linq.Expressions.Expression<Func<PingBook, TDestination>> Configure<TDestination>() where TDestination : new()
    {
       return pingBook => new TDestination
       {
          
       };
    }
}
