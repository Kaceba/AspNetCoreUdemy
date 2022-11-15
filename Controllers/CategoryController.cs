using AspNetCoreUdemy.Data;
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

        public IActionResult Index()
        {
            var objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
    }
}
