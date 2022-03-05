using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Persistence
{
    public interface IRepository<E>
    {
        Task<ICollection<E>> GetAllAsync();
        Task<E> GetAsync(int id);
        Task<E> GetAsync<F>(F id);

        Task<E> GetAsyncWithNoTrack(int id);
        Task<E> GetAsyncWithNoTrack<F>(F id);

        Task<AddItemResult<E>> AddAsync(E entity, string keyName = "Id");

        Task<E> AddAsyncWithoutReturnId(E entity);
        Task<List<E>> AddRangeAsync(List<E> entity);

        //Task<ListMobile<T>> AddRangeAsync(ListMobile<T> entity);
        //Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        //Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        void Update(E entity);
        void Delete(E entity);
        Task DeleteById(int id);

        Task<int> CountAsync();

        //IQueryable<T> AsQueryable();
        //IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        //IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<List<T>> ExecSqlList<T>(string query);
        Task<T> ExecSqlScalar<T>(string query);
        Task BulkCopy(List<E> items);

    }

    public class AddItemResult<T>
    {
        public T entity { get; set; }
        public object Result { get; set; }
    }
    public interface IEntity<T>
    {
        T Id { get; }
    }

    public class Entity<T> : IEntity<T>
    {
        dynamic Item { get; }
        string PropertyName { get; }

        public Entity(dynamic element, string propertyName = "Id")
        {
            Item = element;
            PropertyName = propertyName;

        }
        public T Id
        {
            get
            {
                return (T)Item.Entity.GetType().GetProperty(PropertyName).GetValue(Item.Entity, null);

                //var a = (T)Item.Entity.Id;
                //return a;
            }
        }
    }
}
