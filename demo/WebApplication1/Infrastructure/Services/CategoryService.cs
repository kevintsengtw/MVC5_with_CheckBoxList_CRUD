using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Infrastructure.Repository;
using WebApplication1.Models;

namespace WebApplication1.Infrastructure.Services
{
    public class CategoryService
    {
        private IRepository<Category> _Repository = new DataRepository<Category>();

        public Category RandomOne()
        {
            return this._Repository.FindAll().OrderBy(x => new Guid()).FirstOrDefault();
        }

        public IQueryable<Category> GetAll()
        {
            return this._Repository.FindAll();
        }

    }
}