using System.Threading;
using System.Threading.Tasks;
using GivtAdvertisements.Business.Advertisements;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Advertisements.API.Controllers
{
    [Route("advertisements")]
    public class AdvertisementsController : Controller
    {
        private readonly IMediator _mediator;

        public AdvertisementsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await _mediator.Send(new GetAdvertisementsQuery(),cancellationToken);
            return Ok(new {prop = "ah yeet"});
        }
    }
}