using AspNetCoreUdemy.DataAccess;
using AspNetCoreUdemy.DataAccess.Repository.IRepository;
using AspNetCoreUdemy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace AspNetCoreUdemy.Controllers;
[Area("Admin")]

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public IActionResult Index(string sortOrder, string searchString)
    {
        ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
        ViewBag.DorderSortParam = sortOrder == "Dorder" ? "Dorder_desc" : "Dorder";
        var Categories = from c in _unitOfWork.Category.GetAll()
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
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();

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
        //var categoryFromDb = _db.Categories.Find(id);
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
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
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();

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

        //var categoryFromDb = _db.Categories.Find(id);
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if (categoryFromDbFirst == null)
        {
            return NotFound();
        }

        return View(categoryFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();

        TempData["Success"] = "Category deleted successfully";

        return RedirectToAction("Index");
    }
}


