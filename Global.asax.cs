using ElectronicShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ElectronicShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // Nhớ đăng nhập bằng cookie - không mất khi tắt trình duyệt
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            if (context.Session != null && context.Session["User"] == null && context.Request.Cookies["LoginCookie"] != null)
            {
                var cookie = context.Request.Cookies["LoginCookie"];
                if (cookie != null && cookie["UserId"] != null)
                {
                    int userId = int.Parse(cookie["UserId"]);

                    using (var db = new ElectronicShopContext())
                    {
                        var user = db.Users.Find(userId);
                        if (user != null)
                        {
                            context.Session["User"] = user;
                            context.Session["UserRole"] = user.Role;
                        }
                    }
                }
            }
        }


    }
}
