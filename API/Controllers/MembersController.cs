
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using API.Extensions;
namespace API.Controllers
{
    [Authorize]
    public class MembersController(IMemberRepository memberRepository ) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            return Ok( await memberRepository.GetMembersAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(String id)//localhost:5001/api/members/bob-id
        {
            var member =Ok( await memberRepository.GetMemberByIdAsync(id));
            if (member==null) return NotFound();
            return member;
        }
        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>>GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosForMemberAsync(id));
        }
        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpadteDto memberUpadteDto)
        {
            var memberId=User.GetMemberId();
            var member= await memberRepository.GetMemberForUpdate(memberId);
            if(member==null) return BadRequest("Could not get member");
            member.DisplayName=memberUpadteDto.DisplayName?? member.DisplayName;
            member.Description=memberUpadteDto.Description?? member.Description;
            member.City=memberUpadteDto.City?? member.City;
            member.Country=memberUpadteDto.Country?? member.Country;

            member.User.DisplayName=memberUpadteDto.DisplayName?? member.User.DisplayName;

            // memberRepository.Update(member);
            if(await memberRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update member>>dude are not changing anything wakeup");

        }
    }
}
