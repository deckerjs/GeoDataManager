using System.Collections.Generic;
using System.Security.Claims;

public class AppUser
{
    public AppUser()
    {
        Claims = new List<Claim>();
    }

    public string ID { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string ProviderName { get; set; }
    public string ProviderSubjectId { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Claim> Claims { get; set; }
}