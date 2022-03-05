using LearningContest.Application.Contracts.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RefreshTokenEntity = LearningContest.Domain.Entities.RefreshToken; 

namespace LearningContest.Persistence.Repositories
{
    public class RefreshTokenRepository: General.Repository<RefreshTokenEntity>,
        IRefreshTokenRepository
    {
        public RefreshTokenRepository(
            LearningContestDbContext context,            
            IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool UpdateRefreshToken(int userId, string refreshToken, DateTime expires, string ipAddress)
        {
            var users = _context.RefreshTokens
                .Where(a => a.UserId == userId);
            foreach (var user in users)
            {
                user.Token = refreshToken;
                user.Expires = expires; 
                user.RemoteIpAddress = ipAddress;
                user.Modified = DateTime.Now;
                _context.Entry(user).State = EntityState.Modified; 
            }
            return true; 
        }

        public async Task<bool> AnyRefreshToken(int userId)
        {
            return await _context.RefreshTokens.AnyAsync(a => a.UserId == userId); 
        }

        public async Task<RefreshTokenEntity> FindUserWithRefreshToken(int userId, string refreshToken)
        {
            return await _context.RefreshTokens
                //.Include(a => a.User)
                .Where(a => a.UserId == userId && a.Token == refreshToken)
                .AsNoTracking()
                .FirstOrDefaultAsync(); 
        }

    }
}
