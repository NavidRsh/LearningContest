using System;
using System.Threading.Tasks;
using LearningContest.Application.Contracts;
using LearningContest.Application.Contracts.Persistence;
using LearningContest.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Sieve.Services;
using LearningContest.Application.Contracts.Persistence.Dapper;
using LearningContest.Domain.Common;
using LearningContest.Persistence.Repositories.General;

namespace LearningContest.Persistence.Repositories
{
    public class UnitOfWorkLearningContest : IUnitOfWorkLearningContest
    {
        public LearningContestDbContext _context;
        private IMapper _mapper;
        private readonly SieveProcessor _sieveProcessor;
        private IDbContextTransaction _dbContextTransaction;
        private Func<ConnectionType, IDapperRepository> _dapperRepository;
        public UnitOfWorkLearningContest(LearningContestDbContext context, IMapper mapper,
        SieveProcessor sieveProcessor, Func<ConnectionType, IDapperRepository> dapperRepository)
        {
            _context = context;
            _mapper = mapper;
            _sieveProcessor = sieveProcessor;
            _dapperRepository = dapperRepository;
        }

        public IBasicConfigRepository BasicConfigRepository => new BasicConfigRepository(_context, _sieveProcessor, _mapper);
       
        public IRefreshTokenRepository RefreshTokenRepository => new RefreshTokenRepository(_context, _mapper);
       
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public void CreateTransaction()
        {
            _dbContextTransaction = _context.Database.BeginTransaction();
        }
        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();

        }

        public void ClearTracker()
        {
            //_context.ChangeTracker.Clear();
        }

    }
}
