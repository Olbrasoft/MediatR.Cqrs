namespace MediatR.Cqrs.Common;
public interface ICommand<out TResult> : IRequest<TResult>
{
}
