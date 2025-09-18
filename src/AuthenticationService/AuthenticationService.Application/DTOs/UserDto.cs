namespace AuthenticationService.Application.DTOs;
public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IList<string> Roles { get; set; }

    public UserDto()
    {
        Roles = new List<string>();
    }

    public UserDto(string id, string email, IList<string> roles)
    {
        Id = id;
        Email = email;
        Roles = roles;
    }
}