using System.Linq;
using System.Web.Mvc;
using ElectronicShop.Models;
using ElectronicShop.Filters;
using ElectronicShop.Data;

namespace ElectronicShop.Controllers
{
    [AdminAuthorize]
    public class AdminController : BaseController
    {
        private ElectronicShopContext db = new ElectronicShopContext();

        public ActionResult Dashboard()
        {
            var userCount = db.Users.Count();
            var productCount = db.Products.Count();
            var categoryCount = db.Categories.Count();
            var orderCount = db.Orders.Count();
            var totalRevenue = db.OrderDetails.Sum(od => (decimal?)od.Quantity * od.Price) ?? 0;

            ViewBag.UserCount = userCount;
            ViewBag.ProductCount = productCount;
            ViewBag.CategoryCount = categoryCount;
            ViewBag.OrderCount = orderCount;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }
    }
}
