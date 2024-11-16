using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository;
using E_commerce.Repository.IRepository;
using E_commerce.utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace E_commerce.Controllers
{
   
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender _emailSender; // إضافة IEmailSender

        // المُنشئ
        public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
            this._emailSender = emailSender; // تعيين IEmailSender
        }

        // إضافة منتج إلى السلة
        ApplicationDbContext dbContext =new ApplicationDbContext();
        public IActionResult AddToCart(int id, int quantity)
        {
            var AppUser = userManager.GetUserId(User); // الحصول على ID المستخدم الحالي
            if (AppUser == null)
            {
                return RedirectToAction("Login", "Account"); // إذا لم يكن المستخدم مسجلاً دخول، توجيه إلى صفحة الدخول
            }

            // إنشاء عنصر سلة جديدة
            Cart cart = new Cart()
            {
                Count = quantity,  // عدد المنتج
                productId = id,    // معرف المنتج
                ApplicationUsertId = AppUser // معرف المستخدم
            };

            // التحقق مما إذا كان المنتج موجودًا بالفعل في السلة
            var cartDb = cartRepository.GetOne(expression: e => e.productId == id && e.ApplicationUsertId == AppUser);
            if (cartDb == null)
            {
                cartRepository.create(cart); // إذا لم يكن موجودًا، إضافة المنتج إلى السلة
            }
            else
            {
                cartDb.Count += quantity; // إذا كان موجودًا، زيادة الكمية
            }

            cartRepository.commit(); // حفظ التغييرات في قاعدة البيانات
            TempData["success"] = "Product added to cart"; // إظهار رسالة النجاح

            return RedirectToAction("Index", "Home"); // إعادة توجيه إلى الصفحة الرئيسية
        }

        // عرض محتويات السلة
        public IActionResult Index()
        {
            var appUser = userManager.GetUserId(User); // الحصول على ID المستخدم الحالي
            // جلب جميع العناصر في السلة
            var shoppingCarts = cartRepository.Get([e => e.product, e => e.ApplicationUser], e => e.ApplicationUsertId == appUser);
            ViewBag.TotalPrice = shoppingCarts.Sum(e => e.product.Price * e.Count).ToString("c"); // حساب المجموع الكلي
            return View(shoppingCarts); // عرض السلة
        }

        // زيادة الكمية في السلة
        public IActionResult Increment(int productId)
        {
            var appUser = userManager.GetUserId(User);
            var cartDB = cartRepository.GetOne(expression: e => e.productId == productId && e.ApplicationUsertId == appUser);

            if (cartDB != null)
            {
                cartDB.Count++; // زيادة الكمية
            }

            cartRepository.commit(); // حفظ التغييرات
            return RedirectToAction("Index"); // إعادة تحميل الصفحة
        }

        // تقليل الكمية في السلة
        public IActionResult Dicrement(int productId)
        {
            var appUser = userManager.GetUserId(User);
            var cartDB = cartRepository.GetOne(expression: e => e.productId == productId && e.ApplicationUsertId == appUser);

            if (cartDB != null)
            {
                if (cartDB.Count > 1)
                {
                    cartDB.Count--; // تقليل الكمية
                }
            }
            cartRepository.commit(); // حفظ التغييرات
            return RedirectToAction("Index"); // إعادة تحميل الصفحة
        }

        // حذف منتج من السلة
        public IActionResult Delete(int productId)
        {
            var appUser = userManager.GetUserId(User);
            var cartDB = cartRepository.GetOne(expression: e => e.productId == productId && e.ApplicationUsertId == appUser);

            if (cartDB != null)
            {
                cartRepository.Delete(cartDB); // حذف المنتج من السلة
            }

            cartRepository.commit(); // حفظ التغييرات
            return RedirectToAction("Index"); // إعادة تحميل الصفحة
        }

        // معالجة الدفع باستخدام Stripe
        public IActionResult Pay()
        {
            // إعداد خيارات الدفع
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" }, // تحديد طرق الدفع
                LineItems = new List<SessionLineItemOptions>(), // إضافة عناصر السلة
                Mode = "payment", // وضع الدفع
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Cart/success", // رابط النجاح
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Cart/cancel", // رابط الإلغاء
            };

            var appUser = userManager.GetUserId(User); // الحصول على ID المستخدم الحالي
            var cartDBs = cartRepository.Get([e => e.product, e => e.ApplicationUser], e => e.ApplicationUsertId == appUser);

            foreach (var item in cartDBs)
            {
                options.LineItems.Add(new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp", // تحديد العملة
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.product.Name, // اسم المنتج
                            Description = item.product.Description, // وصف المنتج
                        },
                        UnitAmount = (long)(item.product.Price * 100), // تحويل السعر للقروش/السنتات
                    },
                    Quantity = item.Count, // عدد المنتجات
                });
            }

            if (!options.LineItems.Any())
            {
                TempData["warning"] = "Your cart is empty. Payment was canceled."; // إذا كانت السلة فارغة
                return RedirectToAction("Index", "Home"); // إعادة توجيه إلى الصفحة الرئيسية
            }

            var service = new SessionService();
            var session = service.Create(options); // إنشاء الجلسة مع Stripe

            return Redirect(session.Url); // إعادة توجيه إلى صفحة الدفع
        }

        // النجاح بعد الدفع
        // التعديل في الدالة Success
        [HttpGet("Cart/Success")]
        public async Task<IActionResult> Success()
        {
            var appUser = userManager.GetUserId(User);
            var cartDBs = cartRepository.Get([e => e.product, e => e.ApplicationUser], e => e.ApplicationUsertId == appUser);

            // إنشاء الطلب وتفاصيله في قاعدة البيانات
            var order = new Order
            {
                ApplicationUserId = appUser, // معرّف المستخدم
                OrderDate = DateTime.Now, // تاريخ الطلب
                Status = "Pending", // حالة الطلب
            };

            dbContext.Orders.Add(order); // إضافة الطلب إلى قاعدة البيانات
            dbContext.SaveChanges(); // حفظ التغييرات للحصول على OrderId

            // إضافة تفاصيل الطلب لكل منتج في السلة
            foreach (var item in cartDBs)
            {
                var orderDetails = cartDBs.Select(item => new OrderDetail
                {
                    OrderId = order.OrderId,
                    productId = item.productId,
                    Quantity = item.Count,
                    Price = item.product.Price,
                    Total = item.product.Price * item.Count,
                }).ToList();

                dbContext.OrderDetails.AddRange(orderDetails);

            }

            // حفظ التغييرات الخاصة بتفاصيل الطلب
            dbContext.SaveChanges();

            // إرسال البريد الإلكتروني بعد الدفع
            var user = await userManager.FindByIdAsync(appUser); // جلب بيانات المستخدم
            if (user != null)
            {
                var email = user.Email;
                var subject = "Order Confirmation - 5Shop Site"; // عنوان الرسالة

                // بناء الرسالة في متغير واحد
                var message = "<html><head><style>" +
                    "body { font-family: 'Arial', sans-serif; background-color: #f4f4f4; color: #333; margin: 0; padding: 20px; text-align: right; direction: rtl; }" +
                    "h1 { color: #4A148C; text-align: center; } p { font-size: 1.1em; }" +
                    ".total-price { font-size: 1.2em; font-weight: bold; }" +
                    ".print-button { display: inline-block; padding: 10px 20px; background-color: #8E24AA; color: white; text-decoration: none; font-size: 1.1em; border-radius: 5px; margin-top: 20px; text-align: center; display: block; }" +
                    ".print-button:hover { background-color: #6A1B9A; }" +
                    "</style></head><body>" +
                    "<p>شكرًا على مشترياتك! نحن نقدر دعمك لنا.</p>" +
                    "<p>نأمل أن تستمتع بشرائك. إذا كان لديك أي استفسار، لا تتردد في التواصل معنا.</p>" +
                    "<p><strong>تأكيد الطلب: </strong>تم تأكيد طلبك وهو الآن قيد المعالجة.</p>" +
                    "<p><a href='/Order/PrintInvoice' class='print-button'>طباعة الفاتورة</a></p>" +
                    "<p>شكرًا مرة أخرى لتسوقك معنا!</p></body></html>";

                // إرسال البريد الإلكتروني
                await _emailSender.SendEmailAsync(email, subject, message);
            }

            // حذف العناصر من السلة بعد إضافة الطلب وتفاصيله
            foreach (var item in cartDBs)
            {
                cartRepository.Delete(item); // حذف العنصر من السلة
            }

            cartRepository.commit(); // حفظ التغييرات في قاعدة البيانات
            TempData["success"] = "Payment successful! Cart has been cleared."; // رسالة نجاح الدفع

            return View(); // عرض صفحة النجاح
        }

        // إلغاء الدفع
        [HttpGet("Cart/Cancel")]
        public IActionResult Cancel()
        {
            TempData["error"] = "Payment was canceled. Cart items remain intact."; // رسالة إلغاء الدفع
            return RedirectToAction("Index", "Home"); // إعادة توجيه إلى الصفحة الرئيسية
        }
    }
}
