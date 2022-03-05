using LinqToDB;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Persistence.Extensions
{
    public static class LinqToDbWrapper
    {
        public static async Task BulkCopyAsync<T>(this DbContext context, IAsyncEnumerable<T> entities, BulkCopyOptions options) where T:class
        {
            await context.BulkCopyAsync<T>(options, entities);
        }

        public static async Task DeleteAllAsync<T>(this DbContext context) where T : class
        {
            await context.Set<T>().DeleteAsync();
        }
        public static async Task DeleteAllAsync<T>(this DbContext context, Expression<Func<T, bool>> predicate) where T : class
        {
            await context.Set<T>().DeleteAsync(predicate);
        }
    }
}