using System;
using System.Threading;
using System.Threading.Tasks;
using GivtAdvertisements.Business.Advertisements;
using GivtAdvertisements.Business.Advertisements.Commands;
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
            var advertisements = await _mediator.Send(new GetAdvertisementsQuery(),cancellationToken);
            return Ok(advertisements);
        }

        [HttpPost]
        public async Task<IActionResult> PostNewAdvertisement([FromBody] CreateAdvertisementCommand command, CancellationToken cancellationToken)
        {
            var created = await _mediator.Send(command, cancellationToken);
            return Created($"advertisement/{created.PrimaryKey}", created);
        }

        [HttpHead]
        public async Task<IActionResult> GetLastUpdated(CancellationToken cancellationToken)
        {
            Response.Headers.Add("Last-Modified", DateTime.UtcNow.ToString("o"));
            return Ok();
        }
    }
}