using ActivityPlanner.DTOs;
using ActivityPlanner.Mappings;
using ActivityPlanner.Models;
using ActivityPlanner.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<PlannedActivity>> CreateActivity(CreateActivityDto dto)
        {
            var activity = dto.ToEntity();
            var created = await _activityRepository.CreateActivity(activity);
            return CreatedAtAction(nameof(GetActivityById), new { activityId = created.Id }, created);
        }

        [HttpPut("{activityId}")]
        public async Task<IActionResult> UpdateActivity(int activityId, UpdateActivityDto dto)
        {
            var activity = await _activityRepository.GetActivityById(activityId);
            if (activity == null)
            {
                return NotFound();
            }

            await _activityRepository.UpdateActivity(activity, dto);
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

            await _activityRepository.DeleteActivity(activity);
            return NoContent();
        }
    }
}
