using ElectronicShop.Data;
using ElectronicShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectronicShop.Controllers
{
    public class CartController : BaseController
    {
        private ElectronicShopContext db = new ElectronicShopContext();

        private List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == id);
            if (item != null) cart.Remove(item);

            Session["Cart"] = cart;
            Session["CartCount"] = cart.Sum(i => i.Quantity);
            return RedirectToAction("Checkout");
        }



        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == id);

            if (item != null && quantity > 0)
                item.Quantity = quantity;

            Session["Cart"] = cart;
            Session["CartCount"] = cart.Sum(i => i.Quantity);
            return RedirectToAction("Index");
        }



        public ActionResult Clear()
        {
            Session["Cart"] = new List<CartItem>();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddToCartAjax(int productId)
        {

            var product = db.Products.Find(productId);
            if (product == null) return Json(new { success = false });

            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = 1
                });
            }

            Session["Cart"] = cart;
            Session["CartCount"] = cart.Sum(i => i.Quantity);

            return Json(new
            {
                success = true,
                cartCount = Session["CartCount"]
            });
        }

        public ActionResult Checkout()
        {
            var cart = GetCart();

            if (Session["UserId"] == null)
            {
                TempData["ReturnUrl"] = Url.Action("Checkout", "Cart");
                return RedirectToAction("Login", "Account");
            }


            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(string paymentMethod)
        {
            var cart = GetCart();
            if (cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng rỗng.";
                return RedirectToAction("Checkout");
            }

            var userId = Session["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int id = (int)userId;
            // Sau đó có thể truy vấn từ db nếu cần: db.Users.Find(id)

            
            var order = new Order
            {
                UserId = db.Users.Find(id).Id,
                OrderDate = DateTime.Now,
                Status = "Pending",
                PaymentMethod = paymentMethod,
                TotalPrice = cart.Sum(x => x.Quantity * x.Price),
                OrderDetails = cart.Select(x => new OrderDetail
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            };

            db.Orders.Add(order);
            db.SaveChanges();

            Session["Cart"] = new List<CartItem>();
            Session["CartCount"] = 0;

            return RedirectToAction("OrderSuccess", new { id = order.Id });
        }



        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Include("OrderDetails.Product").FirstOrDefault(o => o.Id == id);
            if (order == null)
                return HttpNotFound();

            return View(order);
        }



    }

}