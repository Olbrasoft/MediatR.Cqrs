using System.Linq.Expressions;

namespace MediatR.Cqrs.FreeSql;
public interface IConfigure<TSource>
{
    Expression<Func<TSource, TDestination>> Configure<TDestination>() where TDestination : new();
}
