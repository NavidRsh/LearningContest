using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace LearningContest.Application.Contracts.Infrastructure
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
