using System;
using System.Security.Claims;

namespace API.Extensions;

public static class ClaimPrincipalExtentions
{
  public static string GetMemberId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
        throw new Exception("connot get memberid from token ");
        
    }
}
