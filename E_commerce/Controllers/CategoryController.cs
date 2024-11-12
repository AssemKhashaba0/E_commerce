using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository;
using E_commerce.Repository.IRepository;
using E_commerce.utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        [Authorize(Roles = $"{SD.AdminRole},{SD.companyRole}")]

        public IActionResult Index()
        {
            var catergory = categoryRepository.GatAll("Products");
            return View(catergory);
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.AdminRole},{SD.companyRole}")]

        public IActionResult Create()
        {
            category category = new category();
            return View(category);
        }
        [HttpPost]
        public IActionResult Create(category category)
        {
            if (ModelState.IsValid)
            {
               categoryRepository.create(category);
                categoryRepository.commit();
                TempData["success"] = "Add category success";

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [HttpGet]
        [Authorize(Roles = $"{SD.AdminRole}")]

        public IActionResult Edit(int categoryid)
        {
            var category = categoryRepository.GetById(categoryid);
            if (category != null)
            {
                return View(category);
            }
            else
            {
                return RedirectToAction("NotFound","Home"); 
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{SD.AdminRole}")]

        public IActionResult Edit(category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Edit(category);
                categoryRepository.commit();
                TempData["success"] = "category updated successfully";

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [Authorize(Roles = $"{SD.AdminRole}")]

        public IActionResult Delete(int categoryId)
        {
            var category = categoryRepository.GetById(categoryId);

            categoryRepository.Delete(category);
            categoryRepository.commit();
            TempData["success"] = "category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

    }
}
