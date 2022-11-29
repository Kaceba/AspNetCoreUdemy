using AspNetCoreUdemy.DataAccess;
using AspNetCoreUdemy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace AspNetCoreUdemy.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.DorderSortParam = sortOrder == "Dorder" ? "Dorder_desc" : "Dorder";
            var Categories = from c in _db.Categories
                             select c;

            if (!String.IsNullOrEmpty(searchString)) 
            {
                Categories = Categories.Where(c => c.Name.Contains(searchString));
            }

            switch (sortOrder) 
            {
                case "Name_desc":
                    Categories = Categories.OrderByDescending(c => c.Name);
                    break;
                case "Dorder":
                    Categories = Categories.OrderBy(c => c.DisplayOrder);
                    break;
                case "Dorder_desc":
                    Categories = Categories.OrderByDescending(c => c.DisplayOrder);
                    break;
                default:
                    Categories = Categories.OrderBy(c => c.Name);
                    break;
            }

            IEnumerable<Category> objCategoryList = Categories;
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("NameEqualDOrder", "The Name can't match the Display Order");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();

                TempData["Success"] = "Category created successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("NameEqualDOrder", "The Name can't match the Display Order");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();

                TempData["Success"] = "Category edited successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Categories.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();

            TempData["Success"] = "Category deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
