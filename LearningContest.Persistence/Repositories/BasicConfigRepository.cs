using LearningContest.Application.Contracts.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sieve.Models;
using LearningContest.Domain.Entities.General;
using LearningContest.Application.Features.BasicConfig.Queries.GetById;

namespace LearningContest.Persistence.Repositories
{
    public class BasicConfigRepository : General.Repository<BasicConfig>,
        IBasicConfigRepository
    {
        private readonly SieveProcessor _sieveProcessor;

        public BasicConfigRepository(
            LearningContestDbContext context,
            SieveProcessor sieveProcessor,
            IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
            _mapper = mapper;
        }

        public async Task<Domain.Entities.General.BasicConfig> GetConfig(CommonInfrastructure.General.Enums.BasicConfigKeyEnum key)
        {
            return await _context.BasicConfigs.Where(a => a.ItemKey == key)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Domain.Entities.General.BasicConfig>> GetModuleConfigs(CommonInfrastructure.General.Enums.ModuleEnum module)
        {
            return await _context.BasicConfigs.Where(a => a.Module == module)
                .ToListAsync();
        }

        public async Task<BasicConfigDto> GetBasicConfigById(int id)
        {
            return await _context.BasicConfigs.Where(a => a.Id == id).Select(a => new BasicConfigDto()
            {
                Id = a.Id,
                ItemValue = a.ItemValue,
                ItemKey = a.ItemKey,
                Description = a.Description,
                Module = a.Module
            }).FirstOrDefaultAsync();
        }

        public async Task<List<BasicConfigDto>> GetSieveList(SieveModel pager)
        {
            var result = await _sieveProcessor.Apply(pager, GetListQuery()).ToListAsync();
            return result;

        }
        public async Task<int> GetSieveCount(SieveModel pager)
        {
            var result = await _sieveProcessor.Apply(pager, GetListQuery(), applyPagination: false).CountAsync();
            return result;
        }

        private IQueryable<BasicConfigDto> GetListQuery()
        {
            return _context.BasicConfigs
                .Select(a => new BasicConfigDto
                {
                    Id = a.Id,
                    ItemValue = a.ItemValue,
                    ItemKey = a.ItemKey,
                    Description = a.Description,
                    Module = a.Module
                });
        }


    }
}
