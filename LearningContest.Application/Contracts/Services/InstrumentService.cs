using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Services
{
    public interface IInstrumentService
    {
    }
    public class InstrumentService : IInstrumentService
    {
        private IMemoryCache _cache;
        private const string instrumentCacheKey = "instruments";
        //private readonly IUnitOfWorkLearningContest _unitOfWork;
        private readonly IServiceScopeFactory _serviceProvider;
        public InstrumentService(IMemoryCache cache, IUnitOfWorkLearningContest unitOfWork, IServiceScopeFactory serviceProvider)
        {
            _cache = cache;
            //_unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }
       
    }
}
