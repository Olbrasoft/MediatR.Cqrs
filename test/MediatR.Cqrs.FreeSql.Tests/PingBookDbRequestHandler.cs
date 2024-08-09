
using FreeSql;
using Olbrasoft.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.Cqrs.FreeSql.Tests;
public class PingBookDbRequestHandler : DbRequestHandler<PingBookDbContext, PingBook, PingBookRequest, string>
{
    public new PingBookDbContext Context => base.Context;
    public new IMapper? Mapper => base.Mapper;
    public new IConfigure<PingBook>? ProjectionConfigurator => base.ProjectionConfigurator;

    public new Task<bool> ExistsAsync(CancellationToken token) => base.ExistsAsync(token);



    public PingBookDbRequestHandler(PingBookDbContext context) : base(context)
    {
    }


    public PingBookDbRequestHandler(IMapper mapper, PingBookDbContext context) : base(mapper, context)
    {
    }

    public PingBookDbRequestHandler(IConfigure<PingBook> projectionConfigurator, PingBookDbContext context) : base(projectionConfigurator, context)
    {
    }

    public override Task<string> Handle(PingBookRequest request, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public new ISelect<PingBook> GetWhere(Expression<Func<PingBook, bool>> condition) => base.GetWhere(condition);

    public new ISelect<PingBook> GetOrderBy<TMember>(Expression<Func<PingBook, TMember>> columnSelector) => base.GetOrderBy(columnSelector);

    public new async Task<PingBook> GetOneOrNullAsync(Expression<Func<PingBook, bool>> condition, CancellationToken token) => await base.GetOneOrNullAsync(condition, token);

    public new async Task<IEnumerable<PingBook>> GetEnumerableAsync(Expression<Func<PingBook, bool>> condition, CancellationToken token) => await base.GetEnumerableAsync(condition, token);

    public new async Task<IEnumerable<PingBook>> GetEnumerableAsync(ISelect<PingBook> select, CancellationToken token) => await base.GetEnumerableAsync(select, token);

    public new async Task<TDestination> GetOneOrNullAsync<TDestination>(Expression<Func<PingBook, bool>> condition, CancellationToken token)  where TDestination : new()
        => await base.GetOneOrNullAsync<TDestination>(condition, token);


}