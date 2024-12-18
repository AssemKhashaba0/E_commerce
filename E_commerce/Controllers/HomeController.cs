using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace E_commerce.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext dbContext =new ApplicationDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string q, int pageNumber = 1)
        {
            IQueryable<product> product = dbContext.products.Include(e => e.category);
            if (pageNumber - 1 <= product.Count() / 5)
            {
                if (q != null)
                {
                    product = product.Where(e => e.Name.Contains(q));
                }
                ViewBag.pageNumber = pageNumber; // ????? pageNumber ??? View
                ViewBag.totalPages = (int)Math.Ceiling((double)product.Count() / 5); // ????? ?????? ??????? ?????
                product = product.Skip((pageNumber - 1) * 5).Take(5);
                return View(product);

            }


            return RedirectToAction("Index", new { q = q, pageNumber = 1 });
        }
        public IActionResult Details(int id)
        {
            
            var product = dbContext.products.Include(p => p.category).FirstOrDefault(p => p.Id == id);
            if (product != null) 
            {
                return View(product);
            }

            else
            {
                return RedirectToAction("NotFound", "Home");
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NotFound()
        {

        return View(); 
        }
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
