using System;
using System.Web;
using System.Web.Mvc;

namespace ElectronicShop.Filters
{
    // Filter này để kiểm tra quyền truy cập theo Role (Admin hoặc User)
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly string role;

        public AuthorizeRoleAttribute(string role)
        {
            this.role = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userId = httpContext.Session["UserId"];
            var userRole = httpContext.Session["UserRole"] as string;

            return userId != null && userRole == role;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            // Lưu lại URL hiện tại để chuyển hướng sau đăng nhập
            session["ReturnUrl"] = filterContext.HttpContext.Request.Url?.PathAndQuery;

            filterContext.Result = new RedirectResult("~/Account/Login");
        }
    }
}
