using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        // if _mediator is null return right !
        // protected here is for drived class can access it.
        protected IMediator Mediator => _mediator ??=
         HttpContext.RequestServices.GetService<IMediator>();
    }
}