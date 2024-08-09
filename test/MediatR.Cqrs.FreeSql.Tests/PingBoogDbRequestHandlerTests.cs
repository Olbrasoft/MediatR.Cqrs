using FluentAssertions;
using FreeSql;
using Moq;
using Olbrasoft.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.Cqrs.FreeSql.Tests;

public class PingBoogDbRequestHandlerTests
{

    private static readonly IFreeSql _freeSql = new FreeSqlBuilder()
        .UseConnectionString(DataType.Sqlite, "Data Source=:memory:")
        .Build();


    private PingBookDbContext CreateContext()
    {
        var options = new DbContextOptions();

        var context = new PingBookDbContext(_freeSql, options);

        return context;
    }

    private PingBookDbContext _context;

    private PingBookDbContext Context
    {
        get
        {

            _context ??= CreateContext();
            return _context;
        }
    }

    //Constructor with parameter context  set property Context
    [Fact]
    public void Constructor_WithParameterContext_SetPropertyContext()
    {
        //Arrange
        var context = new PingBookDbContext();

        //Act
        var handler = new PingBookDbRequestHandler(context);

        //Assert
        handler.Context.Should().Be(context);
    }

    //Constructor with parameter mapper and context set property Mapper and Context
    [Fact]
    public void Constructor_WithParameterMapperAndContext_SetPropertyMapperAndContext()
    {
        //Arrange
        var mapper = new Mock<IMapper>().Object;
        var context = new PingBookDbContext();

        //Act
        var handler = new PingBookDbRequestHandler(mapper, context);

        //Assert
        handler.Mapper.Should().Be(mapper);
        handler.Context.Should().Be(context);
    }

    //Constructor with parameter configurator and context set property ProjectionConfigurator and Context
    [Fact]
    public void Constructor_WithParameterConfiguratorAndContext_SetPropertyProjectionConfiguratorAndContext()
    {
        //Arrange
        var configurator = new Mock<IConfigure<PingBook>>().Object;
        var context = Context;
        //Act
        var handler = new PingBookDbRequestHandler(configurator, context);

        //Assert
        handler.ProjectionConfigurator.Should().Be(configurator);
        handler.Context.Should().Be(context);
    }


    [Fact]
    public async Task ExistsAsync_ReturnTrue_UseConstructorWithParameterContext()
    {
        //Arrange
        var freeSql = _freeSql;

        var options = new DbContextOptions();


        var context = new PingBookDbContext(freeSql, options);



        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.ExistsAsync(default);
        var t = handler.GetWhere(x => x.Title == "Book2");
        var a = t.Count();


        //Assert
        result.Should().BeTrue();
    }





    //GetWhere with parameter condition return result count 1
    [Fact]
    public void GetWhere_WithParameterCondition_ReturnISelectPingBook()
    {

        var context = Context;

        ////Arrange
        var handler = new PingBookDbRequestHandler(context);


        ////Act
        var result = handler.GetWhere(x => x.Title == "Book2");


        ////Assert
        result.Count().Should().Be(1);

    }


    //GetOrderBy with parameter columnSelector c=>c.Title return first book id 3
    [Fact]
    public void GetOrderBy_WithParameterColumnSelector_ReturnISelectPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = handler.GetOrderBy(c => c.Title).First().Id;

        //Assert
        result.Should().Be(3);
    }

    //GetOneOrNullAsync with parameter condition return book with id 1
    [Fact]
    public async Task GetOneOrNullAsync_WithParameterCondition_ReturnPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetOneOrNullAsync(x => x.Id == 1, default);

        //Assert
        result.Id.Should().Be(1);
    }

    //GetEnumerableAsync with parameter condition return 2 books
    [Fact]
    public async Task GetEnumerableAsync_WithParameterCondition_ReturnPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync(x => x.Id > 1, default);

        //Assert
        result.Count().Should().Be(2);
   
    }


    //GetEnumerableAsync with parameter select return 2 books
    [Fact]
    public async Task GetEnumerableAsync_WithParameterSelect_ReturnPingBook()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(context);

        //Act
        var result = await handler.GetEnumerableAsync(handler.GetWhere(x => x.Id > 1), default);

        //Assert
        result.Count().Should().Be(2);
    }

    //GetOneOrNullAsync<PingBookDto> with parameter condition return book with id 1

    [Fact]
    public async Task GetOneOrNullAsync_WithParameterCondition_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var handler = new PingBookDbRequestHandler(  context);

        //Act
        var result = await handler.GetOneOrNullAsync<PingBookDto>(x => x.Id == 1, default);

        //Assert
        result.Title.Should().Be("Book1");
    }

    //Handler with projectionConfigurator GetOneOrNullAsync<PingBookDto>  with parameter condition return book with id 1
    [Fact]
    public async Task Handler_WithProjectionConfigurator_GetOneOrNullAsyncPingBookDto_WithParameterCondition_ReturnPingBookDto()
    {
        //Arrange
        var context = Context;

        var configurator = new PingBookToPingBookDtoConfigurator();

        var handler = new PingBookDbRequestHandler(configurator, context);

        //Act
        var result = await handler.GetOneOrNullAsync<PingBookDto>(x => x.Id == 1, default);

        //Assert
        result.Title.Should().Be("Book1");
    }
    


}
