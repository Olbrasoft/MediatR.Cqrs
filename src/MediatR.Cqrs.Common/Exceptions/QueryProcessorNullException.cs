namespace MediatR.Cqrs.Common.Exceptions;

public class QueryProcessorNullException : ArgumentNullException
{
    public QueryProcessorNullException() : base("processor")
    {

    }
}