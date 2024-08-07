using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.Cqrs.EntityFrameworkCore.Tests;
public class DbRequestHandlerTests
{
    //DbRequestHandler Is abstract Class
    [Fact]
    public void DbRequestHandler_IsAbstractClass()
    {
        //Arrange
        var type = typeof(DbRequestHandler<,,,>);
        //Act
        var isAbstract = type.IsAbstract;
        //Assert
        Assert.True(isAbstract);
    }

}
