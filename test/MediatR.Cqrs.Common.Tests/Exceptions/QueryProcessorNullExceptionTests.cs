using FluentAssertions;

namespace MediatR.Cqrs.Common.Tests.Exceptions;
public class QueryProcessorNullExceptionTests
{
    [Fact]
    public void TypeOf_QueryProcessorNullException_IsPublicShouldBeTrue()
    {
        // Arrange
        var type = typeof(QueryProcessorNullException);

        // Act
        var isPublic = type.IsPublic;

        // Assert
        isPublic.Should().BeTrue();
    }

    [Fact]
    public void TypeOf_QueryProcessorNullException_AssemblyShouldBeSameAsExpected()
    {
        // Arrange
        var expected = typeof(QueryProcessor).Assembly;

        // Act

        var assembly = typeof(QueryProcessorNullException).Assembly;

        // Assert
        assembly.Should().BeSameAs(expected);

    }


    [Fact]
    public void QueryProcessorNullException_WithOutParams_ShouldBeAssingableToArgumentNullException()
    {
        // Arrange
        var expected = typeof(ArgumentNullException);

        // Act
        var sut = new QueryProcessorNullException();


        // Assert
        sut.Should().BeAssignableTo(expected);
    }

    [Fact]
    public void QueryProcessorNullException_WithOutParams_ParamNameShouldBeProcessor()
    {
        // Arrange
        var sut = new QueryProcessorNullException();

        // Act
        var paramName = sut.ParamName;

        // Assert
        paramName.Should().Be("processor");

    }
}
