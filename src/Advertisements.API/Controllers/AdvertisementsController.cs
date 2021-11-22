using System.Threading;
using System.Threading.Tasks;
using GivtAdvertisements.Business.Advertisements;
using GivtAdvertisements.Business.Advertisements.Commands;
using GivtAdvertisements.Business.Advertisements.Models;
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
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            await _mediator.Send(new GetAdvertisementsQuery(),cancellationToken);
            return Ok(new {prop = "ah yeet"});
        }

        [HttpPost]
        public async Task<IActionResult> PostNewAdvertisement([FromBody] CreateAdvertisementCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}