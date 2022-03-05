using System;
using System.Threading.Tasks;
using LearningContest.Application.Contracts;
using LearningContest.Application.Contracts.Persistence;
using LearningContest.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Sieve.Services;

namespace LearningContest.Persistence.Repositories
{
    public class UnitOfWorkAlborz : IUnitOfWorkAlborz
    {
        public LearningContestDbContext _context;
        private IMapper _mapper;
        private readonly SieveProcessor _sieveProcessor;
        private IDbContextTransaction _dbContextTransaction;
        public UnitOfWorkAlborz(LearningContestDbContext context, IMapper mapper,
            SieveProcessor sieveProcessor)
        {
            _context = context;
            _mapper = mapper;
            _sieveProcessor = sieveProcessor;
        }


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
            _dbContextTransaction.Dispose();
        }

        public void ClearTracker()
        {
            _context.ChangeTracker.Clear();
        }

    }
}
