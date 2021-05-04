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
            #region sinh viên
            context.MapRoute("", "hoc-sinh", defaults: new { controller = "Student", action = "Students", area = "Home" });
            context.MapRoute("", "danh-sach-hoc-sinh", defaults: new { controller = "Student", action = "GetPage_Students", area = "Home" });
            context.MapRoute("", "chi-tiet-hoc-sinh/{id}", defaults: new { controller = "Student", action = "Student", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-hoc-sinh", defaults: new { controller = "Student", action = "PostStudent", area = "Home" });
            context.MapRoute("", "xoa-hoc-sinh", defaults: new { controller = "Student", action = "DeleteStudent", area = "Home", id = UrlParameter.Optional });

            #endregion
            #region tài khoản đăng nhập của sinh viên
            context.MapRoute("", "tai-khoan-hoc-sinh", defaults: new { controller = "StudentAccount", action = "StudentAccounts", area = "Home" });
            context.MapRoute("", "danh-sach-tai-khoan-hoc-sinh", defaults: new { controller = "StudentAccount", action = "GetPage_StudentAccounts", area = "Home" });
            context.MapRoute("", "chi-tiet-tai-khoan-hoc-sinh/{id}", defaults: new { controller = "StudentAccount", action = "StudentAccount", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-tai-khoan-hoc-sinh", defaults: new { controller = "StudentAccount", action = "PostStudentAccount", area = "Home" });
            context.MapRoute("", "xoa-tai-khoan-hoc-sinh", defaults: new { controller = "StudentAccount", action = "DeleteStudentAccount", area = "Home", id = UrlParameter.Optional });

            #endregion
            #region hoc phí
            context.MapRoute("", "hoc-phi", defaults: new { controller = "Payment", action = "Payments", area = "Home" });
            context.MapRoute("", "danh-sach-hoc-phi", defaults: new { controller = "Payment", action = "GetPage_Payments", area = "Home" });
            context.MapRoute("", "chi-tiet-hoc-phi/{id}", defaults: new { controller = "Payment", action = "Payment", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-hoc-phi", defaults: new { controller = "Payment", action = "PostPayment", area = "Home" });
            context.MapRoute("", "xoa-hoc-phi", defaults: new { controller = "Payment", action = "DeletePayment", area = "Home", id = UrlParameter.Optional });

            #endregion
        }
    }
}