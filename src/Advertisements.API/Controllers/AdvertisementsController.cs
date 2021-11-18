using Microsoft.AspNetCore.Mvc;

namespace Advertisements.API.Controllers
{
    [Route("advertisements")]
    public class AdvertisementsController : Controller
    {

        public AdvertisementsController()
        {
            
        }
        
        public IActionResult Get()
        {
            return Ok(new {prop = "ah yeet"});
        }
    }
}