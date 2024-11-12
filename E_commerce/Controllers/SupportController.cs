using E_commerce.Data;
using E_commerce.Models;
using E_commerce.utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Controllers
{
    public class SupportController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(SupportMessage supportMessage)
        {
            if (ModelState.IsValid)
            {
                dbContext.SupportMessages.Add(supportMessage);
                dbContext.SaveChanges();
                TempData["success"] = "Thank You!";

                return RedirectToAction("index","Home"); // إعادة توجيه أو عرض رسالة نجاح

            }

            return View(supportMessage); // إعادة عرض الصفحة مع الأخطاء
        }

        [Authorize(Roles = $"{SD.AdminRole}")]

        public IActionResult messages()
        {
            var messages = dbContext.SupportMessages.OrderByDescending(messages => messages.CreatedAt).ToList();
            return View(messages);
        }

    }
    
}
