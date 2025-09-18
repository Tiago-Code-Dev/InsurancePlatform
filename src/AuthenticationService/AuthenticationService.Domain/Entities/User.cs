using Microsoft.AspNet.Identity.EntityFramework;

namespace AuthenticationService.Domain.Entities;

public class User : IdentityUser
{
    public bool IsActive { get; private set; } = true;
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
