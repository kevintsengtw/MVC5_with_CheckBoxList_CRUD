using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Infrastructure.Services;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    public class FooController : Controller
    {
        private readonly CategoryService _categoryService = new CategoryService();

        private List<SelectListItem> CategorySelectListItems
        {
            get
            {
                var categories = this._categoryService.GetAll();
                var items = new List<SelectListItem>();
                foreach (var c in categories)
                {
                    items.Add(item: new SelectListItem()
                    {
                        Value = c.CategoryID.ToString(),
                        Text = c.CategoryName
                    });
                }
                return items;
            }
        }

        public ActionResult Index()
        {
            var items = this.CategorySelectListItems;
            ViewBag.CategoryItems = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Foo instance)
        {
            var items = this.CategorySelectListItems;
            ViewBag.CategoryItems = items;

            if (ModelState.IsValid)
            {
                return View();
            }
            return View(instance);
        }


    }
}