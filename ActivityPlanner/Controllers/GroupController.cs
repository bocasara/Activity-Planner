using ActivityPlanner.DTOs;
using ActivityPlanner.Extensions;
using ActivityPlanner.Mappings;
using ActivityPlanner.Models;
using ActivityPlanner.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ActivityPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly GroupRepository _groupRepository;
        public GroupController(GroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        // temporary stand in for auth
        private int CurrentUserId => int.Parse(Request.Headers["X-User-Id"]!);

        [HttpGet("{groupId}")]
        public async Task<ActionResult<GroupDetailsDto>> GetGroupById(int groupId)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null) {
                return NotFound();
            }
            // if none of the group's member is the caller return 403
            if (!group.Members.Any(m => m.UserId == CurrentUserId)) return StatusCode(403);
            // convert tp Dto
            return Ok(group.ToDetailsDto());
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup(CreateGroupDto dto)
        {
            var group = dto.DtoToEntity(CurrentUserId);
            var created = await _groupRepository.CreateGroup(group);
            var loaded = await _groupRepository.GetGroupById(created.GroupId);
            return CreatedAtAction(nameof(CreateGroup), new { groupId = created.GroupId }, loaded!.ToDetailsDto());
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }
           
            if (!group.IsAdmin(CurrentUserId)) return StatusCode(403);

            await _groupRepository.DeleteGroup(group);
            return NoContent();
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, UpdateGroupDto dto)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }

            if (!group.IsAdmin(CurrentUserId)) return StatusCode(403);

            dto.ApplyTo(group);
            await _groupRepository.UpdateGroup(group);
            return NoContent();
        }

        [HttpDelete("{groupId}/memebers/{userid}")]
        public async Task<IActionResult> KickMember(int groupId, int userId)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null) 
            {
                return NotFound();            
            }
            if (!group.IsAdmin(CurrentUserId)) return StatusCode(403);

            if(userId == CurrentUserId) return BadRequest();

            await _groupRepository.RemoveMember(userId, groupId);
            return NoContent();
        }

        [HttpDelete("{groupId}/members/me")]
        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }

            await _groupRepository.RemoveMember(CurrentUserId ,groupId);
            return NoContent();
        }

       
        
    }
}
