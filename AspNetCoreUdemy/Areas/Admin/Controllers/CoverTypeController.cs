using System.Runtime.CompilerServices;
using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using AspNetCoreUdemy.Models;
using AspNetCoreUdemy.DataAccess;
using AspNetCoreUdemy.DataAccess.Repository.IRepository;

namespace AspNetCoreUdemy.Controllers;
[Area("Admin")]

public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public CoverTypeController(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        this._unitOfWork = unitOfWork;
        this._configuration = configuration;
    }

    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
        return View(objCoverTypeList);
    }

    public IActionResult ADONETIndex()
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        string queryString = "SELECT Id, Name FROM CoverTypes";

        // Create and open the connection in a using block. This
        // ensures that all resources will be closed and disposed
        // when the code exits.

        using (SqlConnection connection =
            new SqlConnection(connectionString))
        {
            // Create the Command and Parameter objects.
            SqlCommand command = new SqlCommand(queryString, connection);

            // Open the connection in a try/catch block.
            // Create and execute the DataReader, writing the result
            // set to the console window.
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                var dt = new DataTable();
                dt.Load(reader);

                IEnumerable<CoverType> CoverTypeList;
                CoverTypeList = (from DataRow dr in dt.Rows
                               select new CoverType()
                               {
                                   Id = Convert.ToInt32(dr["Id"]),
                                   Name = dr["Name"].ToString(),
                               }).ToList();

                return View(CoverTypeList);
            }
            catch (Exception ex)
            {
                ErrorViewModel error = new ErrorViewModel();

                error.RequestId = ex.ToString();

                return View("Error", error);
            }
        }
    }

    //GET
    public IActionResult Create()
    {

        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Add(obj);
            _unitOfWork.Save();

            TempData["Success"] = "CoverType created successfully";

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

        var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        if (CoverTypeFromDbFirst == null)
        {
            return NotFound();
        }

        return View(CoverTypeFromDbFirst);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CoverType obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(obj);
            _unitOfWork.Save();

            TempData["Success"] = "CoverType edited successfully";

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

        var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        if (CoverTypeFromDbFirst == null)
        {
            return NotFound();
        }

        return View(CoverTypeFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();

        TempData["Success"] = "CoverType deleted successfully";

        return RedirectToAction("Index");
    }
}
