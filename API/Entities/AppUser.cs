
namespace API.Entities;

public class AppUser  //auth
{
   public  string Id { get; set; } = Guid.NewGuid().ToString();
   public required string DisplayName { get; set; }
   public required string Email { get; set; }
   public string? ImageUrl { get; set; }
   public required byte[] PasswordHash  { get; set; }

   public required byte[] PasswordSalt  { get; set;}


   // NAV prop
   public Member Member { get; set; } = null!;
}
