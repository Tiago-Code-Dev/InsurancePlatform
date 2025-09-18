using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Interfaces;
public interface IUserAppService
{
    Task<Result<Guid>> RegisterAsync(RegisterUserCommand cmd, CancellationToken ct);
    Task<Result> AssignRoleAsync(AssignRoleCommand cmd, CancellationToken ct);
}
