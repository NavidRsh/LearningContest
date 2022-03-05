using LearningContest.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Persistence.Views
{
    public class CreateOrAlterViewsFromDb
    {
        public async static void CreateOrAlter(string appConnectionString)
        {
            //List<Config> configViewRows = new List<Config>();
            //using (var connection = new SqlConnection(appConnectionString))
            //{
            //    configViewRows = (await connection.QueryAsync<Config>("SELECT * FROM LearningContest.dbo.Configs WHERE Type='View'")).ToList();
            //}
            //foreach (var configViewRow in configViewRows)
            //{
            //    using (var conn = new SqlConnection(appConnectionString))
            //    using (var cmd = conn.CreateCommand())
            //    {
            //        conn.Open();
            //        cmd.CommandText = configViewRow.Value;
            //        cmd.ExecuteNonQuery();
            //    }
            //}

        }
    }
}
