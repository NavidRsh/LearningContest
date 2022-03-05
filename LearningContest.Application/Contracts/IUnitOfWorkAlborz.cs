using LearningContest.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts
{
    public interface IUnitOfWorkAlborz : IDisposable
    {

        void CreateTransaction();
        void Commit();
        void Rollback();
        Task SaveAsync();

        void ClearTracker();
        //IEventRepository EventRepository { get; }
        //IOrderRepository OrderRepository { get; }


    }
}