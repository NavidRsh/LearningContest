using LearningContest.Application.Contracts.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LearningContest.Persistence.Extensions;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
//using LinqToDB.EntityFrameworkCore;
//using LinqToDB.Data;
//using LinqToDB;

namespace LearningContest.Persistence.Repositories.General
{
    public abstract class Repository<E> : IRepository<E>, IDisposable       
       where E : class  //Entity  --> Database
    {
        public LearningContestDbContext _context;
        protected IMapper _mapper;
        private bool _disposed;

        protected Repository(LearningContestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public virtual async Task<ICollection<E>> GetAllAsync()
        {
            return await _context.Set<E>().ToListAsync();
        }

        public async Task<E> GetAsync(int id)
        {
            return await _context.Set<E>().FindAsync(id);

        }
        public async Task<E> GetAsync<F>(F id)
        {
            return await _context.Set<E>().FindAsync(id);

        }
        public async Task<E> GetAsyncWithNoTrack(int id)
        {
            var entity = await _context.Set<E>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
        public async Task<E> GetAsyncWithNoTrack<F>(F id)
        {
            var entity = await _context.Set<E>().FindAsync(id);
            if (entity == null)
            {
                return null;
            }
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
        public async Task<E> AddAsyncWithoutReturnId(E entity)
        {
            await _context.Set<E>().AddAsync(entity);
            return entity;
        }

        public async Task<List<E>> AddRangeAsync(List<E> entity)
        {
            await _context.Set<E>().AddRangeAsync(entity);
            return entity;
        }
        public virtual async Task<AddItemResult<E>> AddAsync(E entity, string keyName = "Id")
        {
            var insertedItem = await _context.Set<E>().AddAsync(entity);
            var s = entity.GetType().GetProperty(keyName);


            string typeName = s.PropertyType.FullName;
            Type typeArgument = Type.GetType(typeName);

            Type genericClass = typeof(Entity<>);
            Type constructedClass = genericClass.MakeGenericType(typeArgument);


            object created = Activator.CreateInstance(constructedClass, insertedItem, keyName);


            return new AddItemResult<E>()
            {
                entity = entity,
                Result = created//new Entity<int>(insertedItem,keyName)
            };
        }
        public async Task BulkCopy(List<E> items)
        {
            await _context.BulkCopyAsync(new BulkCopyOptions
            {
                TableName = _context.Set<E>().ToLinqToDBTable().TableName,
                SchemaName = _context.Set<E>().ToLinqToDBTable().SchemaName
            }, items);
        }

        public void Update(E entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(E entity)
        {
            _context.Set<E>().Remove(entity);
        }

        public async Task DeleteById(int id)
        {
            var item = await _context.Set<E>().FindAsync(id);
            if (item == null)
            {
                throw new Exception(); // ItemNotFound();
            }
            _context.Set<E>().Remove(item);
        }
        //public virtual async Task<D> FindAsync(Expression<Func<D, bool>> predicate)
        //{
        //    return await _context.Set<E>().SingleOrDefaultAsync(predicate);
        //}

        //public async Task<ICollection<D>> FindAllAsync(Expression<Func<D, bool>> predicate)
        //{
        //    return await _context.Set<D>().Where(predicate).ToListAsync();
        //}

        //public virtual async Task<ICollection<D>> FindByAsync(Expression<Func<D, bool>> predicate)
        //{
        //    return await _context.Set<D>().Where(predicate).ToListAsync();
        //}

        public async Task<int> CountAsync()
        {
            
            return await _context.Set<E>().CountAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing) _context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public async Task<List<T>> ExecSqlList<T>(string query)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                _context.Database.OpenConnection();

                List<T> list = new List<T>();
                using (var result = await command.ExecuteReaderAsync())
                {
                    T obj = default(T);
                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in obj.GetType().GetProperties())
                        {
                            if (!object.Equals(result[prop.Name], DBNull.Value))
                            {
                                prop.SetValue(obj, result[prop.Name], null);
                            }
                        }
                        list.Add(obj);
                    }
                }
                _context.Database.CloseConnection();
                return list;
            }
        }

        public async Task<T> ExecSqlScalar<T>(string query)
        {
            DbConnection connection = _context.Database.GetDbConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                _context.Database.OpenConnection();

                if (connection.State.Equals(ConnectionState.Closed)) { connection.Open(); }

                var value = (T)await command.ExecuteScalarAsync();
                _context.Database.CloseConnection();
                return value;
            }
        }

      
    }
}