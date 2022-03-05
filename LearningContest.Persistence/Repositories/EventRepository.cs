//using LearningContest.Application.Contracts.Persistence;
//using LearningContest.Domain.Entities;
//using LearningContest.Persistence.Repositories;
//using AutoMapper;
//using Sieve.Services;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LearningContest.Persistence.Repositories
//{
//    public class EventRepository : BaseRepository<Event>, IEventRepository
//    {
//        private readonly LearningContestDbContext _context;
//        private readonly SieveProcessor _sieveProcessor;
//        private readonly IMapper _mapper;


//        public EventRepository(
//            LearningContestDbContext context,
//            SieveProcessor sieveProcessor,
//            IMapper mapper) : base(context, mapper)
//        {
//            _context = context;
//            _sieveProcessor = sieveProcessor;
//            _mapper = mapper;
//        }

//        public Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
//        {
//            var matches =  _dbContext.Events.Any(e => e.Name.Equals(name) && e.Date.Date.Equals(eventDate.Date));
//            return Task.FromResult(matches);
//        }
//    }
//}
