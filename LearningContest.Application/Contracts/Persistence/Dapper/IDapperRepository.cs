using LearningContest.Domain.DapperPager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Persistence.Dapper
{
    public interface IDapperRepository
    {

        List<T> ExecuteQuery<T>(string query, object parameters);

        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object parameters);


        PagerData<T> ExecuteQueryByPager<T>(string query, object parameters, PagerModel pager);


        Task<PagerData<T>> ExecuteQueryByPagerAsync<T>(string query, object parameters, PagerModel pager);


        T ExecuteQueryFirstOrDefault<T>(string query, object parameters);


        Task<T> ExecuteQueryFirstOrDefaultAsync<T>(string query, object parameters);


        int ExecuteCommand(string command, object parameters);

        Task<int> ExecuteCommandAsync(string command, object parameters);


        T ExecuteCommandScalar<T>(string command, object parameters);


        Task<T> ExecuteCommandScalarAsync<T>(string command, object parameters);
      
      

    }
}
