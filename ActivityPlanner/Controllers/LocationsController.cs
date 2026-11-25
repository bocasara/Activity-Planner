using ActivityPlanner.Extensions;
using ActivityPlanner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetLocations() =>
            Ok(Enum.GetValues<Location>()
                .Select(l => new { Value = l.ToString(), Label = l.ToDisplayName() }));
    }
}