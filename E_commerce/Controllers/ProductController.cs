using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index( string q ,int pageNumber = 1)
        {
            IQueryable<product> product = dbContext.products.Include(e => e.category);
            if (pageNumber -1 <= product.Count()/5)
            {
                if (q != null)
                {
                    product = product.Where(e => e.Name.Contains(q));
                }
                ViewBag.pageNumber = pageNumber; // تمرير pageNumber إلى View
                ViewBag.totalPages = (int)Math.Ceiling((double)product.Count() / 5); // تمرير إجمالي الصفحات أيضًا
                product = product.Skip((pageNumber - 1) * 5).Take(5);
                return View(product);

            }

           
            return RedirectToAction("Index", new { q = q, pageNumber = 1 });
        }
        [HttpGet]
        public IActionResult create()
        {
            var category = dbContext.categories.ToList();
            ViewBag.category = category;
            var product = dbContext.products.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult create(product product, IFormFile ImgUrl)
        {
            if (ImgUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//img", fileName);
                using (var stream = System.IO.File.Create(pathname))
                {
                    ImgUrl.CopyTo(stream);
                }
                product.ImgUrl = fileName;
            }
            dbContext.products.Add(product);
            dbContext.SaveChanges();
            TempData["success"] = "Add product success";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int productId)
        {
            var product = dbContext.products.Find(productId);
            var category = dbContext.categories.ToList();
            ViewBag.allcategory = category;
            if (product != null)
            {
                return View(product);
            }
            else
            {
                return RedirectToAction("NotFound", "Home");
            }
        }



        [HttpPost]
        public IActionResult Edit(product product, IFormFile ImgUrl)
        {

            var oldproduct = dbContext.products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);
            if (ImgUrl != null && ImgUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                var pathold = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", oldproduct.ImgUrl);

                using (var stream = System.IO.File.Create(pathname))
                {
                    ImgUrl.CopyTo(stream);
                }

                if (System.IO.File.Exists(pathold))
                {
                    System.IO.File.Delete(pathold);
                }





                product.ImgUrl = fileName;
            }
            else
            {
                product.ImgUrl = oldproduct.ImgUrl;
            }

            dbContext.products.Update(product);
            dbContext.SaveChanges();

            TempData["success"] = "Product updated successfully";
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Delete(int productId)
        {
            var oldproduct = dbContext.products.AsNoTracking().FirstOrDefault(e => e.Id == productId);


            var pathold = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//img", oldproduct.ImgUrl);


            if (System.IO.File.Exists(pathold))
            {
                System.IO.File.Delete(pathold);
            }




            product product = new product() { Id = productId };

            dbContext.products.Remove(product);
            dbContext.SaveChanges();

            TempData["success"] = "Product deleted successfully.";

            return RedirectToAction(nameof(Index));

        }
    }
}