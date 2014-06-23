using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Infrastructure.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Add(T entity);

        void Delete(T entity);

        void Delete(Func<T, bool> predicate);

        void Update(T entity);

        bool IsExists(Func<T, bool> predicate);

        void Attach(T entity);

        T Single(Func<T, bool> predicate);

        T SingleOrDefault(Func<T, bool> predicate);

        T First(Func<T, bool> predicate);

        T FirstOrDefault(Func<T, bool> predicate);

        IQueryable<T> Find(Func<T, bool> predicate);

        IQueryable<T> FindAll();

        int Count();

        int Count(Func<T, bool> predicate);

        void SaveChanges();

    }
}
