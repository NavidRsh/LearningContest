using LearningContest.Application.Contracts.Persistence.Dapper;
using LearningContest.Domain.DapperPager;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LearningContest.Persistence.Repositories.Dapper
{

    public class AlborzDapperRepository : DapperRepository
    {
        public AlborzDapperRepository(string cs) : base(cs)
        {
        }
    }

    public class LearningContestDapperRepository : DapperRepository
    {
        public LearningContestDapperRepository(string cs) : base(cs)
        {
        }
    }


    public class DapperRepository : IDapperRepository
    {
        public string _connectionString { get; }

        public DapperRepository(string cs)
        {
            _connectionString = cs;
        }

        public List<T> ExecuteQuery<T>(string query, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<T>(query, parameters).ToList();
            }
        }
        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<T>(query, parameters);
            }
        }

        public PagerData<T> ExecuteQueryByPager<T>(string query, object parameters, PagerModel pager)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var countQuery = $@"SELECT COUNT(1) FROM ({query}) Q";
                var totalCount = connection.ExecuteScalar<int>(countQuery, parameters);

                var pagerResult = connection.Query<T>(getPagerQuery(query, pager), parameters);

                int pageCount = totalCount / pager.PageSize;
                if (totalCount > 0 && (totalCount % pager.PageSize) > 0)
                {
                    pageCount++;
                }

                return new PagerData<T>(pagerResult, totalCount, pageCount, pager.Page, pager.PageSize);
            }
        }

        public async Task<PagerData<T>> ExecuteQueryByPagerAsync<T>(string query, object parameters, PagerModel pager)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var countQuery = $@"SELECT COUNT(1) FROM ({query}) Q";
                var totalCount = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

                var pagerResult = await connection.QueryAsync<T>(getPagerQuery(query, pager), parameters);

                return new PagerData<T>(pagerResult, totalCount, pager.TotalPageCount, pager.Page, pager.PageSize);
            }
        }

        private string getPagerQuery(string query, PagerModel pager)
        {
            var skip = (pager.Page - 1) * pager.PageSize;

            // return string.Format(@"
            // SELECT * FROM ({0}) Q
            // ORDER BY {1}
            // OFFSET {2} ROWS
            // FETCH NEXT {3} ROWS ONLY", query, getSortClause(pager.SortBy), skip, pager.PageSize);

            string resultQuery = string.Empty;
            if (string.IsNullOrEmpty(pager.Sorts))
            {
                resultQuery = @$"
WITH  QResult AS
(SELECT Q.*, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum FROM ({query}) Q)
SELECT
  *
FROM QResult
WHERE RowNum BETWEEN {skip + 1} AND {skip + pager.PageSize}";
            }
            else
            {
                resultQuery = string.Format(@"
WITH QResult AS
(SELECT Q.*, ROW_NUMBER() OVER (ORDER BY {1}) AS RowNum FROM ({0}) Q)
SELECT * 
FROM QResult
WHERE RowNum BETWEEN {2} AND {3}", query, getSortClause(pager.Sorts), skip + 1, skip + pager.PageSize);
            }

            return resultQuery;

        }


        public T ExecuteQueryFirstOrDefault<T>(string query, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<T>(query, parameters);
            }
        }

        public async Task<T> ExecuteQueryFirstOrDefaultAsync<T>(string query, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
            }
        }

        public int ExecuteCommand(string command, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Execute(command, parameters);
            }
        }

        public async Task<int> ExecuteCommandAsync(string command, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteAsync(command, parameters);
            }
        }

        public async Task ExecuteTransactionCommandAsync(List<(string command, string parameters)> commands)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var transactionScope = new TransactionScope())
                {
                    foreach (var command in commands)
                    {
                        await connection.ExecuteAsync(command.command, command.parameters);
                    }
                    transactionScope.Complete();
                }
            }
        }

        public T ExecuteCommandScalar<T>(string command, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return (T)connection.ExecuteScalar(command, parameters);
            }
        }


        public async Task<T> ExecuteCommandScalarAsync<T>(string command, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.ExecuteScalarAsync<T>(command, parameters);
            }
        }



        private string getSortClause(string sortByString)
        {
            var sortBy = (string.IsNullOrWhiteSpace(sortByString) ? "Id" : sortByString.Trim()).ToLower();
            var sortArr = sortBy.Split(',');

            var sortExp = string.Empty;

            foreach (var s in sortArr)
            {
                var desc = s.StartsWith("-");
                var columnName = desc ? s.Substring(1) : s;
                if (columnName.StartsWith("-"))
                {
                    desc = true;
                    columnName = columnName.Substring(5);
                }
                sortExp += columnName + (desc ? " desc," : ",");
            }
            return sortExp.Substring(0, sortExp.Length - 1);
        }
    }

}
