using LearningContest.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts
{
    public interface IUnitOfWorkLearningContest : IDisposable
    {

        void CreateTransaction();
        void Commit();
        void Rollback();
        //void Save();
        Task SaveAsync();

        void ClearTracker();
        //IProductSerialRepository ProductSerialRepository { get; }
        //IProductSerialDetailRepository ProductSerialDetailRepository { get; }
        //ICategoryRepository CategoryRepository { get; }
        //IEventRepository EventRepository { get; }
        //IOrderRepository OrderRepository { get; }

        
        IBasicConfigRepository BasicConfigRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
    }
}