using System.Linq;
using System.Web.Mvc;
using ElectronicShop.Data;
using ElectronicShop.Models;
using ElectronicShop.Helpers;
using System;
using System.Web;

namespace ElectronicShop.Controllers
{
    public class AccountController : BaseController
    {
        private ElectronicShopContext db = new ElectronicShopContext();

        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl, bool RememberMe = false)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null && user.Password == PasswordHelper.HashPassword(model.Password))
                {
                    // Lưu session
                    Session["UserId"] = user.Id;
                    Session["UserRole"] = user.Role;

                    if (RememberMe)
                    {
                        // Tạo cookie đăng nhập
                        HttpCookie userCookie = new HttpCookie("LoginCookie");
                        userCookie["UserId"] = user.Id.ToString();
                        userCookie["Role"] = user.Role;
                        userCookie.Expires = DateTime.Now.AddDays(7); // sống 7 ngày
                        Response.Cookies.Add(userCookie);
                    }

                    // Chuyển hướng về trang trước (nếu có returnUrl)
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // Nếu không có returnUrl thì kiểm tra quyền
                    if (user.Role == "Admin")
                        return RedirectToAction("Dashboard", "Admin");
                    else
                        return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            }

            return View(model);
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();

            if (Request.Cookies["LoginCookie"] != null)
            {
                var cookie = new HttpCookie("LoginCookie");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");
        }

        // QUÊN MẬT KHẨU (RESET PASSWORD)
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    // Reset mật khẩu tạm thời
                    string tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
                    user.Password = PasswordHelper.HashPassword(tempPassword);
                    db.SaveChanges();

                    // Gửi email (giả lập hoặc gửi thật nếu có SMTP)
                    TempData["ResetPassword"] = tempPassword;
                    return RedirectToAction("ResetConfirmation", new { email = model.Email });
                }

                ModelState.AddModelError("", "Email không tồn tại.");
            }

            return View(model);
        }

        // Hiển thị thông báo mật khẩu tạm (giả lập gửi mail)
        public ActionResult ResetConfirmation(string email)
        {
            ViewBag.Email = email;
            ViewBag.TempPassword = TempData["ResetPassword"];
            return View();
        }

        // ĐỔI MẬT KHẨU
        public ActionResult ChangePassword()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login");

            int userId = (int)Session["UserId"];
            var user = db.Users.Find(userId);

            if (PasswordHelper.HashPassword(model.OldPassword) != user.Password)
            {
                ModelState.AddModelError("", "Mật khẩu cũ không đúng.");
                return View();
            }

            user.Password = PasswordHelper.HashPassword(model.NewPassword);
            db.SaveChanges();

            ViewBag.Message = "Đổi mật khẩu thành công.";
            return View();
        }

        // HỒ SƠ NGƯỜI DÙNG
        public ActionResult UserProfile()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login");

            int userId = (int)Session["UserId"];
            var user = db.Users.Find(userId);

            return View(user);
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(model);
                }

                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = PasswordHelper.HashPassword(model.Password),
                    Phone = model.Phone,
                    Address = model.Address,
                    Role = "User",
                    CreatedAt = DateTime.Now
                };

                db.Users.Add(user);
                db.SaveChanges();

                Session["UserId"] = user.Id;
                Session["UserRole"] = user.Role;

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }

}

