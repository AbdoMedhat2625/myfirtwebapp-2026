using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context,ITokenServices tokenServices):BaseApiController
{
  [HttpPost("register")]//api/account/register
  public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
    {
        if(await EmailExists(registerDto.Email)) return BadRequest("Email taken ^-^");
        using  var hmac = new HMACSHA512();

        var user = new AppUser
        {
            DisplayName=registerDto.DisplayName,
            Email=registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt= hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.ToDto(tokenServices);
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x=>x.Email==loginDto.Email);
        if(user==null) return Unauthorized("wrong email maybe u can try again if not sign up");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for(var i = 0; i < ComputeHash.Length; i++)
        {
            if(ComputeHash[i]!=user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
       return user.ToDto(tokenServices);

    }
        private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x=>x.Email.ToLower()==email.ToLower());
    }
    
}
