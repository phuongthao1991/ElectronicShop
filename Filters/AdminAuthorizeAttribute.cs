using System.Web;
using System.Web.Mvc;

namespace ElectronicShop.Filters
{
    // Filter này chỉ cho Admin đang nhập
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userId = httpContext.Session["UserId"];
            var role = httpContext.Session["UserRole"] as string;

            return userId != null && role == "Admin";
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            // Nếu chưa đăng nhập hoặc không phải Admin
            if (session["UserId"] == null || session["UserRole"]?.ToString() != "Admin")
            {
                // Lưu lại URL hiện tại để quay lại sau khi đăng nhập
                var request = filterContext.HttpContext.Request;
                session["ReturnUrl"] = request.Url?.PathAndQuery;

                // Chuyển đến trang Login
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
