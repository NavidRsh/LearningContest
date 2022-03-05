using LearningContest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Persistence
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<bool> AnyRefreshToken(int userId);
        bool UpdateRefreshToken(int userId, string refreshToken, DateTime expires, string ipAddress);
        Task<RefreshToken> FindUserWithRefreshToken(int userId, string refreshToken); 
    }
}
