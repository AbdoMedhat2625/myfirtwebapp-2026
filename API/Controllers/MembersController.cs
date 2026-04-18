
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using API.Extensions;
using API.Helpers;
namespace API.Controllers
{
    [Authorize]
    public class MembersController(IMemberRepository memberRepository ,
    IPhotoService photoService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers([FromQuery]MemberParams memberParams)
        {
            memberParams.CurrentMemberId=User.GetMemberId();
            return Ok( await memberRepository.GetMembersAsync(memberParams));
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
        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm]IFormFile file)
        {
            var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if(member==null) return BadRequest("Cannot update member");
            var result = await photoService.UploadPhotoAsync(file);
            if(result.Error!=null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId= result.PublicId,
                MemberId= User.GetMemberId()
            };
            if (member.ImageUrl == null)
            {
                member.ImageUrl=photo.Url;
                member.User.ImageUrl=photo.Url;

            }
            member.Photos.Add(photo);
            if(await memberRepository.SaveAllAsync()) return photo;
            return BadRequest("Problem adding photo");

        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member= await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if(member==null) return BadRequest("Cannot get member from token");
            var photo = member.Photos.FirstOrDefault(x=>x.Id==photoId);
            if(member.ImageUrl==photo?.Url || photo ==null)
            {
                return BadRequest("Cannot set thsi as main image");
            }
            member.ImageUrl=photo.Url;
            member.User.ImageUrl=photo.Url;

            if(await memberRepository.SaveAllAsync()) return NoContent();
            return BadRequest("problem setting mian photo");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeltePhoto(int photoId)
        {
            var member= await memberRepository.GetMemberForUpdate(User.GetMemberId());
            if(member==null) return BadRequest("Cannot get member from token");
            var photo = member.Photos.FirstOrDefault(x=>x.Id==photoId);
            if(photo==null||photo.Url==member.ImageUrl)
            {
                return BadRequest("this photo cannot be deleted");

            }
            if(photo.PublicId!=null)
            {
                var result = await photoService.DeletePhotoAync(photo.PublicId);
                if(result.Error!=null) return BadRequest(result.Error.Message);
            }
            member.Photos.Remove(photo);
            if(await memberRepository.SaveAllAsync()) return Ok();
            return BadRequest("problem deleting photos");
        }
    }
}
