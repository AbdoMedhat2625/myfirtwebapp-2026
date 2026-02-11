using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenServices
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw  new Exception("cant get token key ");
        if (tokenKey.Length< 64) 
             throw new Exception("your token keys need to be bigger or equal 64 chars");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email,user.Email),
            new(ClaimTypes.NameIdentifier,user.Id)
        };

        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
        var tokenDescripter = new SecurityTokenDescriptor
        {
            Subject=new ClaimsIdentity(claims),
            Expires =DateTime.UtcNow.AddDays(7),
            SigningCredentials=creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescripter);
        return tokenHandler.WriteToken(token);

    }
}
