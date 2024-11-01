using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext dbContext =new ApplicationDbContext();
        public IActionResult Index()
        {
            var product = dbContext.products. Include(e=>e.category).ToList();
            return View(product);
        }
        [HttpGet]
        public IActionResult create()
        {
            var category = dbContext.categories.ToList();
            return View(category);
        }
        [HttpPost]
        public IActionResult create(product product , IFormFile  ImgUrl)
        {
            if(ImgUrl.Length > 0)
            {
                var fileName = ImgUrl.FileName;
               var pathname = Path.Combine(Directory.GetCurrentDirectory() ,"wwwroot//img", fileName);
                using (var stream = System.IO.File.Create(pathname))
                {
                    ImgUrl.CopyTo(stream);
                }
                product.ImgUrl=ImgUrl.FileName;
            }
            dbContext.products.Add(product);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
