using Sieve.Models;
using LearningContest.Application.Features.BasicConfig.Queries.GetById;
using LearningContest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Persistence
{
    public interface IBasicConfigRepository : IRepository<Domain.Entities.General.BasicConfig>
    {
        Task<Domain.Entities.General.BasicConfig> GetConfig(CommonInfrastructure.General.Enums.BasicConfigKeyEnum key);
        Task<List<Domain.Entities.General.BasicConfig>> GetModuleConfigs(CommonInfrastructure.General.Enums.ModuleEnum module);

        Task<BasicConfigDto> GetBasicConfigById(int id);

        Task<List<BasicConfigDto>> GetSieveList(SieveModel pager);

        Task<int> GetSieveCount(SieveModel pager);


    }
}
