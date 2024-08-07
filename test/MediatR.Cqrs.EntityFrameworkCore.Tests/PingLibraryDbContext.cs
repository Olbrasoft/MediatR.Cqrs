using Microsoft.EntityFrameworkCore;

namespace MediatR.Cqrs.EntityFrameworkCore.Tests;

public class PingLibraryDbContext : DbContext
{
    public PingLibraryDbContext(DbContextOptions options) : base(options)
    {
   
    }

    public DbSet<PingBook> Books { get; set; }


}