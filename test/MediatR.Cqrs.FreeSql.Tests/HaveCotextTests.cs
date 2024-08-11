using FreeSql;

namespace MediatR.Cqrs.FreeSql.Tests;

public class HaveCotextTests : IDisposable
{
    protected static readonly IFreeSql _freeSql = new FreeSqlBuilder()
    .UseConnectionString(DataType.Sqlite, "Data Source=:memory:")
    .Build();


    public void Dispose()
    {
        _context?.Dispose();
        GC.SuppressFinalize(this);
    }

    private PingBookDbContext CreateContext()
    {
        var options = new DbContextOptions();

        var context = new PingBookDbContext(_freeSql, options);

        return context;
    }

    private PingBookDbContext? _context;

    protected PingBookDbContext Context
    {
        get
        {

            _context ??= CreateContext();
            return _context;
        }
    }


}