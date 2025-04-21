using System;
using System.Web;
using System.Web.Mvc;
using ElectronicShop.Data;

namespace ElectronicShop.Controllers
{
    public class BaseController : Controller
    {
        protected ElectronicShopContext db = new ElectronicShopContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Nếu chưa có Session thì phục hồi từ cookie
            if (Session["UserId"] == null && Request.Cookies["LoginCookie"] != null)
            {
                var cookie = Request.Cookies["LoginCookie"];
                if (cookie != null && cookie["UserId"] != null)
                {
                    Session["UserId"] = int.Parse(cookie["UserId"]);
                    Session["UserRole"] = cookie["Role"];
                }
            }

            // Lấy thông tin người dùng hiện tại
            if (Session["UserId"] != null)
            {
                int userId = (int)Session["UserId"];
                var user = db.Users.Find(userId);
                ViewBag.CurrentUser = user; // Truyền user cho layout
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
