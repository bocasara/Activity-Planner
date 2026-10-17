using Microsoft.AspNetCore.Mvc;
using ActivityPlanner.Repositories;
using ActivityPlanner.Models;
using ActivityPlanner.Data;


namespace ActivityPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
       
        private readonly ActivityRepository _activityRepository;
        public ActivitiesController(ActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlannedActivity>>> GetActivities()
        {
            return Ok(await _activityRepository.GetActivities());
        }

        [HttpGet("{activityId}")]

        public async Task<ActionResult<PlannedActivity>> GetActivityById(int activityId)
        {
            var activity = await _activityRepository.GetActivityById(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        [HttpPost]
        public async Task<ActionResult<PlannedActivity>> CreateActivity(PlannedActivity newActivity)
        {
            if (newActivity == null)
            {
                return BadRequest();
            }

            await _activityRepository.CreateActivity(newActivity);
            return CreatedAtAction(nameof(GetActivityById), new { activityId = newActivity.Id }, newActivity);

        }


        [HttpPut("{activityId}")]
        public async Task<IActionResult> UpdateActivity(int activityId, PlannedActivity updatedActivity)
        {
            if (updatedActivity == null)
            {
                return BadRequest();
            }

            var activity = await _activityRepository.GetActivityById(activityId);
            if (activity == null)
            {
                return NotFound();
            }
            await _activityRepository.UpdateActivity(activityId, updatedActivity);

            return NoContent();
        }

        [HttpDelete("{activityId}")]
        public async Task<IActionResult> DeleteActivity(int activityId)
        {
            var activity = await _activityRepository.GetActivityById(activityId);
            if (activity == null)
            {
                return NotFound();
            }
            await _activityRepository.RemoveActivity(activityId);
            return NoContent();

        }
    }
}
