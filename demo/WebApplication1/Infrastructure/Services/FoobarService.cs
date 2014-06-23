using System;
using System.Linq;
using WebApplication1.Infrastructure.Repository;
using WebApplication1.Models;

namespace WebApplication1.Infrastructure.Services
{
    public class FoobarService
    {
        private IRepository<Foobar> _Repository = new DataRepository<Foobar>();

        public void Create(Foobar instance)
        {
            if(instance == null)
            {
                throw new ArgumentNullException("instance", "null");
            }
            instance.ID = Guid.NewGuid();
            instance.CreateDate = DateTime.Now;
            instance.UpdateDate = DateTime.Now;

            this._Repository.Add(instance);
            this._Repository.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var instance = this.GetOne(id);
            if (instance == null)
            {
                throw new InvalidOperationException("not found");
            }

            this._Repository.Delete(instance);
            this._Repository.SaveChanges();
        }

        public void Update(Foobar instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance", "null");
            }

            instance.UpdateDate = DateTime.Now;

            this._Repository.Update(instance);
            this._Repository.SaveChanges();
        }

        public bool IsExists(Guid id)
        {
            return this._Repository.IsExists(x => x.ID == id);
        }

        public Foobar GetOne(Guid id)
        {
            return this._Repository.FirstOrDefault(x => x.ID == id);
        }

        public IQueryable<Foobar> GetAll()
        {
            return this._Repository.FindAll().OrderByDescending(x => x.CreateDate);
        }


    }
}