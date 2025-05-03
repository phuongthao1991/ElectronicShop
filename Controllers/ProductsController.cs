using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectronicShop.Data;
using ElectronicShop.Models;
using ElectronicShop.Filters;
using System.IO;
using ElectronicShop.Helpers;
using PagedList; // Thêm namespace

namespace ElectronicShop.Controllers
{
    [AdminAuthorize]
    public class ProductsController : BaseController
    {
       
        private ElectronicShopContext db = new ElectronicShopContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {

            var products = db.Products.Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        [AllowAnonymous] 
        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            // kiểm tra Admin để điều hướng cho đúng
            // admin vào view CURD
            // User vào trang chi tiết xem hàng của user
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,CategoryId,Description,Price,Stock,ImageUrl,CreatedAt")] Product product,
            HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // Tạo tên ảnh mới
                    var ext = Path.GetExtension(ImageFile.FileName);
                    var newFileName = Path.GetFileNameWithoutExtension(product.Name) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Products"), newFileName);

                    // Lưu file
                    //ImageFile.SaveAs(path);
                    // Resize ảnh
                    ImageHelper.ResizeAndSaveImage(ImageFile, path, 200, 130); // ví dụ: 300x300 px

                    // Gán tên file vào CSDL
                    product.ImageUrl = newFileName;
                }

                // Lưu thời gian tạo nếu chưa có
                if (product.CreatedAt == DateTime.MinValue)
                {
                    product.CreatedAt = DateTime.Now;
                }

                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product, HttpPostedFileBase NewImage)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = db.Products.Find(product.Id);
                if (existingProduct == null)
                    return HttpNotFound();

                // Cập nhật các thông tin khác
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.CreatedAt = product.CreatedAt;

                // Nếu có ảnh mới
                if (NewImage != null && NewImage.ContentLength > 0)
                {
                    // Xoá ảnh cũ
                    var oldPath = Server.MapPath("~/Content/Images/Products/" + existingProduct.ImageUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);

                    // Tạo tên ảnh mới
                    var ext = Path.GetExtension(NewImage.FileName);
                    var newFileName = Path.GetFileNameWithoutExtension(product.Name) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                    var newPath = Path.Combine(Server.MapPath("~/Content/Images/Products"), newFileName);

                    //NewImage.SaveAs(newPath);
                    // Resize ảnh
                    ImageHelper.ResizeAndSaveImage(NewImage, newPath, 200, 130); // ví dụ: 200x130 px

                    existingProduct.ImageUrl = newFileName;
                }

                db.Entry(existingProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.FindAsync(id);

            if (product != null)
            {
                // Đường dẫn ảnh
                string imagePath = Server.MapPath("~/Content/Images/Products/" + product.ImageUrl);

                // Xóa file nếu tồn tại
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                // Xóa bản ghi sản phẩm
                db.Products.Remove(product);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        // Xem sản phẩm theo danh mục
        [AllowAnonymous]
        public ActionResult ByCategory(int categoryId, int page = 1, int pageSize = 6)
        {
            var products = db.Products
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.Id) // Sắp xếp mới nhất
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalProducts = db.Products.Count(p => p.CategoryId == categoryId);

            ViewBag.CurrentCategory = db.Categories.Find(categoryId);
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.CategoryId = categoryId;

            return View(products);
        }

        // Load dữ liệu cho view tạo danh mục
        [AllowAnonymous]
        [ChildActionOnly] // Giúp action chỉ được gọi từ view (không truy cập trực tiếp từ trình duyệt)
        public PartialViewResult CategoryMenu()
        {
            var categories = db.Categories.ToList();
            return PartialView("_CategoryMenu", categories);
        }

        //
        [AllowAnonymous]
        public ActionResult Search(string keyword, int? categoryId, int page = 1, int pageSize = 6)
        {
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Name.Contains(keyword));
                ViewBag.Keyword = keyword;
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId);
                ViewBag.CategoryId = categoryId;
            }

            var categories = db.Categories.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);

            var pagedProducts = products.OrderBy(p => p.Name).ToPagedList(page, pageSize);

            return View(pagedProducts);
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
