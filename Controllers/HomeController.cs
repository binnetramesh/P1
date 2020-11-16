using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1.Common;
using P1.Models;

namespace P1.Controllers
{
    public class HomeController : Controller
    {
        #region Private Variables
        private readonly ILogger<HomeController> _logger;
        private DbOperations _dbOperations = new DbOperations();
        #endregion

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Inventory()
        {
            List<Inventory> lst = _dbOperations.GetAllInventoryList("");
            return View(lst);
        }
        [HttpGet]
        public IActionResult CreateInventory()
        {
            Inventory obj = new Inventory();
            obj.CountryList = _dbOperations.GetAllCountryList("");
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateInventory([Bind] Inventory obj)
        {
            if (ModelState.IsValid)
            {
                _dbOperations.AddInventory(obj);
                return RedirectToAction("Inventory");
            }
            else
            {
                return View(obj);
            }
        }

        #region Country
        //public JsonResult Inventory()
        //{
        //    List<Inventory> lst = _dbOperations.GetAllInventoryList("");
        //    return View(lst);
        //}
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
