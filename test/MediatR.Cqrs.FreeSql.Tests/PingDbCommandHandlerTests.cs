using Moq;
using Olbrasoft.Mapping;

namespace MediatR.Cqrs.FreeSql.Tests;
public class PingDbCommandHandlerTests
{
    ////UseAutoChangeCommandStatus set property command
    //[Fact]
    //public void UseAutoChangeCommandStatus_SetPropertyCommand()
    //{
    //    //Arrange
    //    var command = new PingDbCommand();
    //    var handler = new PingDbCommandHandler(new PingBookDbContext());

    //    //Act
    //    handler.UseAutoChangeCommandStatus(command);

    //    //Assert
    //    Assert.Equal(command, handler.Command);
    //}

    ////TrySetCommandStatus return true
    //[Fact]
    //public void TrySetCommandStatus_ReturnTrue()
    //{
    //    //Arrange
    //    var handler = new PingDbCommandHandler(new PingBookDbContext());
    //    handler.UseAutoChangeCommandStatus(new PingDbCommand());

    //    //Act
    //    var result = handler.TrySetCommandStatus(CommandStatus.Created);

    //    //Assert
    //    Assert.True(result);
    //}

    ////TrySetCommandStatus return false
    //[Fact]
    //public void TrySetCommandStatus_ReturnFalse()
    //{
    //    //Arrange
    //    var handler = new PingDbCommandHandler(new PingBookDbContext());

    //    //Act
    //    var result = handler.TrySetCommandStatus(CommandStatus.Created);

    //    //Assert
    //    Assert.False(result);
    //}

    //MapTo verify call MapTo on mock mapper


    [Fact]
    public void MapTo_VerifyCallMapToOnMockMapper()
    {
        //Arrange
        var mapper = new Mock<IMapper>();
        var handler = new PingDbCommandHandler(mapper.Object, new PingBookDbContext());

        //Act
        handler.MapTo<PingBookDto>(new PingBook());

        //Assert
        mapper.Verify(m => m.MapTo<PingBookDto>(It.IsAny<PingBook>()), Times.Once);
    }

    //MapCommandToNewEntity verify call MapSourceToNewDestination on mock mapper
    [Fact]
    public void MapCommandToNewEntity_VerifyCallMapSourceToNewDestinationOnMockMapper()
    {
        //Arrange
        var mapper = new Mock<IMapper>();
        var handler = new PingDbCommandHandler(mapper.Object, new PingBookDbContext());

        //Act
        handler.MapCommandToNewEntity(new PingDbCommand());

        //Assert
        mapper.Verify(m => m.MapTo<PingBook>(It.IsAny<PingDbCommand>()), Times.Once);
    }


    //MapCommandToExistingEntity verify call Map source, destination on mock mapper
    [Fact]
    public void MapCommandToExistingEntity_VerifyCallMapSourceDestinationOnMockMapper()
    {
        //Arrange
        var mapper = new Mock<IMapper>();
        var handler = new PingDbCommandHandler(mapper.Object, new PingBookDbContext());

        //Act
        handler.MapCommandToExistingEntity(new PingDbCommand(), new PingBook());

        //Assert
        mapper.Verify(m => m.Map(It.IsAny<PingDbCommand>(), It.IsAny<PingBook>()), Times.Once);
    }

    //CallThrowIfCommandIsNullOrCancellationRequested Throw ArgumentNullException when command is null
    [Fact]
    public void CallThrowIfCommandIsNullOrCancellationRequested_ThrowArgumentNullExceptionWhenCommandIsNull()
    {
        //Arrange
        var handler = new PingDbCommandHandler(new PingBookDbContext());

        //Act
#pragma warning disable IDE0062 // Make local function 'static'
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        void Act() => PingDbCommandHandler.CallThrowIfCommandIsNullOrCancellationRequested(null, CancellationToken.None);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore IDE0062 // Make local function 'static'

        //Assert
        Assert.Throws<ArgumentNullException>(Act);
    }

    //CallThrowIfCommandIsNullOrCancellationRequested Throw OperationCanceledException when token is canceled
    [Fact]
    public void CallThrowIfCommandIsNullOrCancellationRequested_ThrowOperationCanceledExceptionWhenTokenIsCanceled()
    {
        //Arrange
        var handler = new PingDbCommandHandler(new PingBookDbContext());

        //Act
#pragma warning disable IDE0062 // Make local function 'static'
        void Act() => PingDbCommandHandler.CallThrowIfCommandIsNullOrCancellationRequested(new PingDbCommand(), new CancellationToken(true));
#pragma warning restore IDE0062 // Make local function 'static'

        //Assert
        Assert.Throws<OperationCanceledException>(Act);
    }
}
