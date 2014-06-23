using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Infrastructure.Repository
{
    public class DataRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The context object for the database
        /// </summary>
        public DbContext _context;

        /// <summary>
        /// The IObjectSet that represents the current entity.
        /// </summary>
        public DbSet<T> _dbSet;

        /// <summary>
        /// Initializes a new instance of the DataRepository class
        /// </summary>
        public DataRepository()
            : this(new NorthwindEntities())
        {
        }

        /// <summary>
        /// Initializes a new instance of the DataRepository class
        /// </summary>
        /// <param name="context">The Entity Framework ObjectContext</param>
        public DataRepository(DbContext context)
        {
            _context = context;
            _dbSet = this._context.Set<T>();
        }

        //=========================================================================================

        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this._dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbSet.Remove(entity);
        }

        public void Delete(Func<T, bool> predicate)
        {
            IEnumerable<T> records = from x in _dbSet.Where<T>(predicate) select x;

            foreach (T record in records)
            {
                _dbSet.Remove(record);
            }
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }


        public bool IsExists(Func<T, bool> predicate)
        {
            return this._dbSet.FirstOrDefault<T>(predicate) != null;
        }

        public void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        public T Single(Func<T, bool> predicate)
        {
            return _dbSet.Single<T>(predicate);
        }

        public T SingleOrDefault(Func<T, bool> predicate)
        {
            return _dbSet.SingleOrDefault<T>(predicate);
        }

        public T First(Func<T, bool> predicate)
        {
            return _dbSet.First<T>(predicate);
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return _dbSet.FirstOrDefault<T>(predicate);
        }

        public IQueryable<T> Find(Func<T, bool> predicate)
        {
            return _dbSet.Where<T>(predicate).AsQueryable();
        }

        public IQueryable<T> FindAll()
        {
            return this._dbSet.AsQueryable();
        }

        public int Count()
        {
            return this._dbSet.Count();
        }

        public int Count(Func<T, bool> predicate)
        {
            return this._dbSet.Count(predicate);
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) { return; }
            if (this._context == null) { return; }
            this._context.Dispose();
            this._context = null;
        }
    }
}