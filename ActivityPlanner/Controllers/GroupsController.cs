using ActivityPlanner.DTOs;
using ActivityPlanner.Extensions;
using ActivityPlanner.Mappings;
using ActivityPlanner.Models;
using ActivityPlanner.Repositories;
using ActivityPlanner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController(GroupRepository groupRepository, UserRepository userRepository, RecommendationService recommendationService, ActivityRepository activityRepository) : ControllerBase
    {
        private readonly GroupRepository _groupRepository = groupRepository;
        private readonly UserRepository _userRepository = userRepository;
        private readonly RecommendationService _recommendationService = recommendationService;
        private readonly ActivityRepository _activityRepository = activityRepository;

        private int CurrentUserId =>
                   int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                       ??
                       User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)!.Value);

        [HttpGet("{groupId}")]
        public async Task<ActionResult<GroupDetailsDto>> GetGroupById(int groupId)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null) {
                return NotFound();
            }
            // if none of the group's member is the caller return 403
            if (!group.IsApprovedMember(CurrentUserId)) return StatusCode(403);
            // convert tp Dto
            return Ok(group.ToDetailsDto(CurrentUserId));
        }

        [HttpPost]
        public async Task<ActionResult<GroupDetailsDto>> CreateGroup(CreateGroupDto dto)
        {
            var group = dto.DtoToEntity(CurrentUserId);
            var created = await _groupRepository.CreateGroup(group);
            var loaded = await _groupRepository.GetGroupById(created.GroupId);
            return CreatedAtAction(nameof(GetGroupById), new { groupId = created.GroupId }, loaded!.ToDetailsDto(CurrentUserId));
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
       

        [HttpDelete("{groupId}/members/{userid}")]
        public async Task<IActionResult> KickMember(int groupId, int userId)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null) 
            {
                return NotFound();            
            }
            if (!group.IsAdmin(CurrentUserId)) return StatusCode(403);

            if(userId == CurrentUserId) return BadRequest();

            if( !await _groupRepository.RemoveMember(userId, groupId) )
            {
                return NotFound();
            }
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

            if (!group.Members.Any(m => m.UserId == CurrentUserId)) return NotFound();

            if (group.IsLastAdmin(CurrentUserId))
            {
                if(group.Members.Count == 1) 
                {
                  await _groupRepository.DeleteGroup(group);
                    return NoContent();

                }
                return Conflict("You are the last admin of this group and cannot leave it Promote another member before leaving.");
            }
            

            await _groupRepository.RemoveMember(CurrentUserId ,groupId);
            return NoContent();
        }

        [HttpPost("join/{invitationCode}")]
        public async Task<ActionResult<GroupDetailsDto>> JoinGroup(string invitationCode)
        {
            var group = await _groupRepository.GetGroupByInvitationCode(invitationCode);
            if (group == null)
            {
                return NotFound();
            }

            if (group.Members.Any(m => m.UserId == CurrentUserId))
            {
                return Conflict("You are already a member of this group.");
            }

            var groupMember = group.ToGroupMember(CurrentUserId);
            await _groupRepository.AddMember(groupMember);

            var loaded = await _groupRepository.GetGroupById(group.GroupId);
            return Ok(loaded!.ToDetailsDto(CurrentUserId));
        }

        [HttpPost("{groupId}/members")]
        public async Task<IActionResult> AddMember(int groupId, AddMemberDto dto)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null)
            {
                return NotFound();
            }
            if (!group.IsAdmin(CurrentUserId)) return StatusCode(403);

            var user = await _userRepository.GetUserByUsername(dto.Username);
            if (user == null) return NotFound("User not found.");

            if (group.Members.Any(m => m.UserId == user.UserId))
                return Conflict("User is already a member of this group.");

            var groupMember = group.ToGroupMember(user.UserId);
            await _groupRepository.AddMember(groupMember);

            var loaded = await _groupRepository.GetGroupById(group.GroupId);
            return Ok(loaded!.ToDetailsDto(CurrentUserId));
        }

        [HttpPut("{groupId}/preferences")]
        public async Task<IActionResult> SetPreferences(int groupId, GroupPreferenceDto dto)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null) return NotFound();
            if (!group.IsAdmin(CurrentUserId))
                return StatusCode(403);
            group.Location = dto.Location;
            group.Budget = dto.Budget;

            await _groupRepository.UpdateGroup(group);
            return Ok(group.ToDetailsDto(CurrentUserId));
        }

        [HttpGet("{groupId}/recommendations")]
        public async Task<ActionResult<List<RecommendedActivityDto>>> GetRecommendations(int groupId, decimal? budget, Location? location)
        {
            var group = await _groupRepository.GetGroupById(groupId);
            if (group == null) return NotFound();

            if (budget == null || location == null)
                return BadRequest("A budget and a location are required.");
            if (budget < 0)
                return BadRequest("Budget cannot be negative.");
            var users = group.Members
               .Where(m => m.Status == MembershipStatus.Approved)
               .Select(m => m.User);
            var activities = await _activityRepository.GetActivities();

            return Ok(_recommendationService.Recommend(users, activities, budget.Value, location.Value));
        }
       
    }
}
