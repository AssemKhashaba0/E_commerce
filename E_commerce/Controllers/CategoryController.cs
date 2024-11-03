using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var catergory = dbContext.categories.ToList();  
            return View(catergory);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(category category)
        {
            dbContext.categories.Add(category);
            dbContext.SaveChanges();
            TempData["success"] = "Add category success";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int categoryid)
        {
            var category = dbContext.categories.Find(categoryid);
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
        public IActionResult Edit(category category)
        {
            dbContext.categories.Update(category);
            dbContext.SaveChanges();
            TempData["success"] = "category updated successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int categoryId)
        {
            category category = new category() { Id = categoryId };
            dbContext.categories.Remove(category);
            dbContext.SaveChanges();
            TempData["success"] = "category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

    }
}
