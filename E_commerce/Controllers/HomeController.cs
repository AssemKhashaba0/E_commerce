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

        public IActionResult Index()
        {
            var product = dbContext.products.ToList();
            return View(product);
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
