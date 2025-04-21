using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectronicShop.Data;

namespace ElectronicShop.Controllers
{
    public class HomeController : BaseController
    {
        private ElectronicShopContext db = new ElectronicShopContext();
        public ActionResult Index()
        {
            // Sản phẩm mới (lấy theo ngày tạo giảm dần)
            var newProducts = db.Products.OrderByDescending(p => p.CreatedAt).Take(8).ToList();

            // Sản phẩm bán chạy (dựa vào số lượng đã bán từ OrderDetails)
            // Truy vấn tổng số lượng bán của mỗi sản phẩm (group by và sum trong bộ nhớ)
            var bestSellersGrouped = db.OrderDetails
                .GroupBy(od => od.ProductId)  // Nhóm theo ProductId
                .Select(g => new
                {
                    ProductId = g.Key,         // Lấy ProductId
                    TotalQuantity = g.Sum(od => od.Quantity)  // Tổng số lượng bán
                })
                .ToList();  // Lưu kết quả vào bộ nhớ

            // Truy vấn danh sách sản phẩm tương ứng với các ProductId đã nhóm
            var bestSellersProductIds = bestSellersGrouped
                .OrderByDescending(b => b.TotalQuantity)  // Sắp xếp theo tổng số lượng
                .Take(8)  // Lấy top 8 sản phẩm bán chạy nhất
                .Select(b => b.ProductId)  // Lấy danh sách ProductId
                .ToList();

            // Lấy thông tin chi tiết sản phẩm từ bảng Products
            var bestSellers = db.Products
                .Where(p => bestSellersProductIds.Contains(p.Id))  // Chọn các sản phẩm có trong danh sách ProductId
                .ToList();  // Truy vấn sản phẩm từ database

            // Trả kết quả về ViewBag hoặc Model
            ViewBag.BestSellers = bestSellers;
            ViewBag.NewProducts = newProducts;

            ViewBag.Categories = db.Categories.ToList(); // Thêm dòng này

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}