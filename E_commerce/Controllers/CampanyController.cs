using E_commerce.Data;
using E_commerce.Models;
using E_commerce.utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class CampanyController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        [Authorize(Roles = $"{SD.AdminRole},{SD.companyRole}")]

        public IActionResult Index()
        {
            var Campany = dbContext.Campanies. Include(e=>e.Products).ToList();
            return View(Campany);
        }
        [HttpGet]
        [Authorize(Roles = $"{SD.AdminRole},{SD.companyRole}")]

        public IActionResult Create()
        {
            Campany campany = new Campany();
            return View(campany);
        }
        [Authorize(Roles = $"{SD.AdminRole},{SD.companyRole}")]

        [HttpPost]
        public IActionResult Create(Campany campany)
        {
            if (ModelState.IsValid)
            {

                dbContext.Campanies.Add(campany);
                dbContext.SaveChanges();
                TempData["success"] = "Add campany success";

                return RedirectToAction("Index");
            }
            return View(campany);
        }
        [Authorize(Roles = $"{SD.AdminRole}")]

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var campany = dbContext.Campanies.Find(id);
            if (campany != null)
            {
                return View(campany);
            }
            else
            {
                return RedirectToAction("NotFound", "Home");

            }
        }
        [HttpPost]
        public IActionResult Edit(Campany campany)
        {
            if (ModelState.IsValid)
            {

                dbContext.Campanies.Update(campany);
                dbContext.SaveChanges();
                TempData["success"] = "campany updated successfully";

                return RedirectToAction("Index");
            }
            return View(campany);

        }
        [Authorize(Roles = $"{SD.AdminRole}")]

        public IActionResult Delete(int campanyId)
        {
            Campany campany = new Campany() { Id = campanyId };
            dbContext.Campanies.Remove(campany);
            dbContext.SaveChanges();
            TempData["success"] = "Company deleted successfully.";

            return RedirectToAction(nameof(Index));
        }


    }
}
