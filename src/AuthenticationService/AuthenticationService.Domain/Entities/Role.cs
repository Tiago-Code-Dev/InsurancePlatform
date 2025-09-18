using Microsoft.AspNet.Identity.EntityFramework;

namespace AuthenticationService.Domain.Entities;

public class Role : IdentityRole
{
    public string Description { get; set; }
}
