using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas
{
    public class HomeAreaRegistration : AreaRegistration
    {
        public override string AreaName {
            get {
                return "Home";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("", "trang-chu", defaults: new { controller = "Home", action = "Index", area = "Home" });


            #region giáo viên
            context.MapRoute("", "giao-vien", defaults: new { controller = "Teacher", action = "Teacheres", area = "Home" });
            context.MapRoute("", "chi-tiet-giao-vien/{id}", defaults: new { controller = "Teacher", action = "Teacher", area = "Home" ,id= UrlParameter.Optional});
            #endregion
        }
    }
}