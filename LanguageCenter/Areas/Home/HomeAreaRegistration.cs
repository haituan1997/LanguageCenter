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
            context.MapRoute("", "danh-sach-iao-vien", defaults: new { controller = "Teacher", action = "GetTeacheres", area = "Home" });
            context.MapRoute("", "chi-tiet-giao-vien/{id}", defaults: new { controller = "Teacher", action = "Teacher", area = "Home" ,id= UrlParameter.Optional});
            context.MapRoute("", "luu-giao-vien/{id}", defaults: new { controller = "Teacher", action = "PostTeacher", area = "Home" ,id= UrlParameter.Optional});
            context.MapRoute("", "xoa-giao-vien", defaults: new { controller = "Teacher", action = "DeleteTeacher", area = "Home" ,id= UrlParameter.Optional});
            #endregion
            #region học sinh
            context.MapRoute("", "hoc-sinh", defaults: new { controller = "Student", action = "Students", area = "Home" });
            context.MapRoute("", "chi-tiet-hoc-sinh/{id}", defaults: new { controller = "Student", action = "Student", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-hoc-sinh", defaults: new { controller = "Student", action = "PostStudent", area = "Home" });
            #endregion
        }
    }
}