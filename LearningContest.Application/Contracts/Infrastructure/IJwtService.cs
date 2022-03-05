using LearningContest.Domain.Common.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Infrastructure
{
    public interface IJwtService
    {
        Task<AccessTokenDto> GenerateEncodedToken(int userId, string userName, long? customerId, string optionalIssuer = "", string optionalAudience = "", string optionalSecretKey = "");

        string GenerateRefreshToken(int size = 32); 
    }
}
