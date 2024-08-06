using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR.Cqrs;
public abstract class BaseRequest<TResult> : IRequest<TResult> 
{
    public IMediator Mediator { get; set; }
   

    protected BaseRequest(IMediator mediator)
    {
        Mediator = mediator ?? throw new ArgumentNullException();
    }

}




