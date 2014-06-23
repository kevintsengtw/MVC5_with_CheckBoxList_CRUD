using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Infrastructure.Services;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FoobarsController : Controller
    {
        private FoobarService foobarService = new FoobarService();
        private CategoryService categoryService = new CategoryService();

        private List<SelectListItem> CategorySelectListItems(string selected = "")
        {
            var categories = this.categoryService.GetAll();
            var items = new List<SelectListItem>();

            var selectedCategories = string.IsNullOrWhiteSpace(selected)
                ? null
                : selected.Split(',');

            foreach (var c in categories)
            {
                items.Add(item: new SelectListItem()
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName,
                    Selected = selectedCategories == null
                        ? false
                        : selectedCategories.Contains(c.CategoryID.ToString())
                });
            }
            return items;
        }

        public ActionResult Index()
        {
            var foobars = this.foobarService.GetAll();
            var categories = this.CategorySelectListItems();

            var result = new List<Foobar>();
            foreach ( var item in foobars )
            {
                var selectedList = item.Categories.Split(',').ToList();
                item.SelectedCategories = string.IsNullOrWhiteSpace(item.Categories)
                    ? ""
                    : string.Join(
                        ",",
                        categories.Where(x => selectedList.Contains(x.Value)).Select(x => x.Text));

                result.Add(item);
            }

            return View(result.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foobar foobar = this.foobarService.GetOne(id.Value);
            if (foobar == null)
            {
                return HttpNotFound();
            }

            var items = this.CategorySelectListItems(foobar.Categories);
            foobar.SelectedCategories = string.IsNullOrWhiteSpace(foobar.Categories)
                ? ""
                : string.Join(",", items.Where(x => x.Selected).Select(x => x.Text)); 

            return View(foobar);
        }

        public ActionResult Create()
        {
            var items = this.CategorySelectListItems();
            ViewBag.CategoryItems = items;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, string[] categories)
        {
            var foobar = new Foobar
            {
                Name = name,
                Categories = string.Join(",", categories)
            };

            if (ModelState.IsValid)
            {
                this.foobarService.Create(foobar);
                return RedirectToAction("Index");
            }

            var items = this.CategorySelectListItems();
            ViewBag.CategoryItems = items;

            return View(foobar);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foobar foobar = this.foobarService.GetOne(id.Value);
            if (foobar == null)
            {
                return HttpNotFound();
            }

            var items = this.CategorySelectListItems(foobar.Categories);
            ViewBag.CategoryItems = items;

            return View(foobar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, string name, string[] categories)
        {
            var foobar = this.foobarService.GetOne(id);
            if (foobar == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                foobar.Name = name;
                foobar.Categories = string.Join(",", categories);
                this.foobarService.Update(foobar);

                return RedirectToAction("Index");
            }

            var items = this.CategorySelectListItems();
            ViewBag.CategoryItems = items;

            return View(foobar);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foobar foobar = this.foobarService.GetOne(id.Value);
            if (foobar == null)
            {
                return HttpNotFound();
            }

            var items = this.CategorySelectListItems(foobar.Categories);
            foobar.SelectedCategories = string.IsNullOrWhiteSpace(foobar.Categories)
                ? ""
                : string.Join(",", items.Where(x => x.Selected).Select(x => x.Text)); 

            return View(foobar);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            bool isExists = this.foobarService.IsExists(id);
            if (!isExists)
            {
                return RedirectToAction("Index");
            }
            this.foobarService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
