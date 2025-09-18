using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Interfaces;
public interface ITokenGenerator { string GenerateRefreshToken(); }
