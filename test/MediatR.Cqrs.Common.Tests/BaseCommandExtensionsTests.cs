﻿namespace MediatR.Cqrs.Common.Tests;
public class BaseCommandExtensionsTests
{
    [Fact]
    public async void ToResultAsync_WhenCommandIsNull_ShouldThrowAsyncCommandNullException()
    {
        // Arrange
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        BaseCommand<object> baseCommand = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => baseCommand.ToResultAsync();
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        await act.Should().ThrowAsync<CommandNullException>();
    }

    [Fact]
    public async void ToResultAsync_WhenCommandWithExcutor_VerifyCallMethodExecuteAsyncOnTheExecutorOnce()
    {
        // Arrange
        var execMock = new Mock<ICommandExecutor>();
        var sut = new BaseCommand<object>(execMock.Object);
        // Act
        await sut.ToResultAsync();
        // Assert
        execMock.Verify(e => e.ExecuteAsync(sut, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void ToResultAsync_WhenCommandWithDispatcher_VerifyCallMethodDispatchAsyncOnTheDispatcherOnce()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        var sut = new BaseCommand<object>(mediator.Object);
        // Act
        await sut.ToResultAsync();
        // Assert
        mediator.Verify(d => d.Send(sut, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void ToResultAsync_WhenComandHaveDispatcherAndExecutorIsNull_ShouldThrowAsyncToResultException()
    {
        // Arrange
        var sut = new PingCommand();
        // Act
        var act = () => sut.ToResultAsync();
        // Assert
        await act.Should().ThrowAsync<ToResultException>();
    }

    [Fact]
    public async void ToResultAsync_WhenCommandHaveDispatcherAndExecutorIsNull_ShouldThrowAsyncToresultExceptionAndMessageContainExecutor()
    {
        // Arrange
        var sut = new PingCommand();
        // Act
        var act = () => sut.ToResultAsync();
        // Assert
        await act.Should().ThrowExactlyAsync<ToResultException>().Where(ex => ex.Message.Contains("Executor"));
    }
}
