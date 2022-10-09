using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.TasksManagement.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
