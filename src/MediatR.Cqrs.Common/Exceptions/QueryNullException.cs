namespace MediatR.Cqrs.Common.Exceptions;

public class QueryNullException : ArgumentNullException
{
    public QueryNullException() : base("query")
    {
    }
}