using ElectronicShop.Data;
using ElectronicShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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



        //[HttpPost]
        //public ActionResult UpdateQuantity(int id, int quantity)
        //{
        //    var cart = GetCart();
        //    var item = cart.FirstOrDefault(i => i.ProductId == id);

        //    if (item != null && quantity > 0)
        //        item.Quantity = quantity;

        //    Session["Cart"] = cart;
        //    Session["CartCount"] = cart.Sum(i => i.Quantity);
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public ActionResult UpdateQuantity(Dictionary<int, int> quantities, string action)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null) return RedirectToAction("Index");

            if (!string.IsNullOrEmpty(action))
            {
                var parts = action.Split('-');
                if (parts.Length == 2)
                {
                    var type = parts[0];
                    if (int.TryParse(parts[1], out int productId))
                    {
                        var item = cart.FirstOrDefault(p => p.ProductId == productId);
                        if (item != null)
                        {
                            if (type == "increase") item.Quantity++;
                            else if (type == "decrease" && item.Quantity > 1) item.Quantity--;
                        }
                    }
                }
            }

            Session["Cart"] = cart;
            return RedirectToAction("Checkout");
        }



        public ActionResult Clear()
        {
            Session["Cart"] = new List<CartItem>();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult AddToCartAjax(int productId)
        //{

        //    var product = db.Products.Find(productId);
        //    if (product == null) return Json(new { success = false });

        //    var cart = GetCart();
        //    var item = cart.FirstOrDefault(i => i.ProductId == productId);
        //    if (item != null)
        //    {
        //        item.Quantity++;
        //    }
        //    else
        //    {
        //        cart.Add(new CartItem
        //        {
        //            ProductId = product.Id,
        //            ProductName = product.Name,
        //            Price = product.Price,
        //            ImageUrl = product.ImageUrl,
        //            Quantity = 1
        //        });
        //    }

        //    Session["Cart"] = cart;
        //    Session["CartCount"] = cart.Sum(i => i.Quantity);

        //    return Json(new
        //    {
        //        success = true,
        //        cartCount = Session["CartCount"]
        //    });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddToCartAjax(int productId, int quantity = 1)

        {

            var product = db.Products.Find(productId);
            if (product == null || quantity <= 0)
            {
                return Json(new { success = false });
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = quantity
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Checkout(string paymentMethod)
        //{
        //    var cart = GetCart();
        //    if (cart.Count == 0)
        //    {
        //        TempData["Error"] = "Giỏ hàng rỗng.";
        //        return RedirectToAction("Checkout");
        //    }

        //    var userId = Session["UserId"];
        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    int id = (int)userId;
        //    // Sau đó có thể truy vấn từ db nếu cần: db.Users.Find(id)


        //    var order = new Order
        //    {
        //        UserId = db.Users.Find(id).Id,
        //        OrderDate = DateTime.Now,
        //        Status = "Pending",
        //        PaymentMethod = paymentMethod,
        //        TotalPrice = cart.Sum(x => (decimal)x.Quantity * x.Price),
        //        OrderDetails = cart.Select(x => new OrderDetail
        //        {
        //            ProductId = x.ProductId,
        //            Quantity = x.Quantity,
        //            Price = x.Price
        //        }).ToList()
        //    };

        //    db.Orders.Add(order);
        //    db.SaveChanges();

        //    Session["Cart"] = new List<CartItem>();
        //    Session["CartCount"] = 0;

        //    return RedirectToAction("OrderSuccess", new { id = order.Id });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(string paymentMethod)
        {
            var userId = Session["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Tính tổng tiền
                    decimal total = cart.Sum(item => item.Price * item.Quantity);

                    // Tạo đơn hàng
                    var order = new Order
                    {
                        UserId = Convert.ToInt32(userId),
                        TotalPrice = total,
                        Status = "Pending",
                        PaymentMethod = paymentMethod,
                        OrderDate = DateTime.Now
                    };
                    db.Orders.Add(order);
                    db.SaveChanges(); // Cần lưu để có OrderId

                    // Thêm chi tiết đơn hàng và cập nhật tồn kho
                    foreach (var item in cart)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price
                        };
                        db.OrderDetails.Add(orderDetail);

                        // Trừ tồn kho
                        var product = db.Products.Find(item.ProductId);
                        if (product != null)
                        {
                            if (product.Stock < item.Quantity)
                            {
                                throw new Exception($"Sản phẩm '{product.Name}' không đủ hàng.");
                            }

                            product.Stock -= item.Quantity;
                            db.Entry(product).State = EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                    dbContextTransaction.Commit();

                    // Xóa giỏ hàng sau khi đặt hàng thành công
                    //Session["Cart"] = null;
                    Session["Cart"] = new List<CartItem>();
                    Session["CartCount"] = 0;

                    return RedirectToAction("OrderSuccess", new { id = order.Id });
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    TempData["Error"] = "Có lỗi xảy ra khi đặt hàng: " + ex.Message;
                    return RedirectToAction("Checkout");
                }
            }
        }




        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Include("OrderDetails.Product").FirstOrDefault(o => o.Id == id);
            if (order == null)
                return HttpNotFound();

            return View(order);
        }

        // nút lênh mua ngay
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyNow(int productId, int quantity = 1)
        {
            var product = db.Products.Find(productId);
            if (product == null || quantity <= 0)
            {
                TempData["Error"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = quantity
                });
            }

            Session["Cart"] = cart;
            Session["CartCount"] = cart.Sum(i => i.Quantity);

            return RedirectToAction("Checkout");
        }


    }

}