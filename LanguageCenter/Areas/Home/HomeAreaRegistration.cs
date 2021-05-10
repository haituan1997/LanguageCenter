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
            #region home
            context.MapRoute("", "trang-chu", defaults: new { controller = "Home", action = "Index", area = "Home" });
            context.MapRoute("", "khoa-hoc/{id}", defaults: new { controller = "Home", action = "Course", area = "Home" ,id = UrlParameter.Optional});

            context.MapRoute("", "lich-hoc", defaults: new { controller = "HomeClassWeekDay", action = "ClassWeekDay", area = "Home" });
            context.MapRoute("", "lich-hoc-theo-lop/{classID}", defaults: new { controller = "HomeClassWeekDay", action = "FullCalendar", area = "Home" ,classID = UrlParameter.Optional });


            #endregion


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
            #region khóa đào tạo
            context.MapRoute("", "khoa-dao-tao", defaults: new { controller = "Course", action = "Courses", area = "Home" });
            context.MapRoute("", "danh-sach-khoa-dao-tao", defaults: new { controller = "Course", action = "GetPage_Courses", area = "Home" });
            context.MapRoute("", "chi-tiet-khoa-dao-tao/{id}", defaults: new { controller = "Course", action = "Course", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-khoa-dao-tao", defaults: new { controller = "Course", action = "PostCourse", area = "Home" });
            context.MapRoute("", "xoa-khoa-dao-tao", defaults: new { controller = "Course", action = "DeleteCourse", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-danh-muc", defaults: new { controller = "Course", action = "PostCategory", area = "Home" });
            context.MapRoute("", "luu-ngon-ngu", defaults: new { controller = "Course", action = "PostLanguage", area = "Home" });
            context.MapRoute("", "luu-cap-do", defaults: new { controller = "Course", action = "PostLevel", area = "Home" });
            context.MapRoute("", "load-ngon-ngu", defaults: new { controller = "Course", action = "GetLanguages", area = "Home" });
            context.MapRoute("", "load-danh-muc", defaults: new { controller = "Course", action = "GetCategories", area = "Home" });
            context.MapRoute("", "load-cap-do", defaults: new { controller = "Course", action = "GetLevels", area = "Home" });
            #endregion
            #region tài khoản đăng nhập của giáo viên
            context.MapRoute("", "tai-khoan-giao-vien", defaults: new { controller = "TeacherAccount", action = "TeacherAccounts", area = "Home" });
            context.MapRoute("", "danh-sach-tai-khoan-giao-vien", defaults: new { controller = "TeacherAccount", action = "GetPage_TeacherAccounts", area = "Home" });
            context.MapRoute("", "chi-tiet-tai-khoan-giao-vien/{id}", defaults: new { controller = "TeacherAccount", action = "TeacherAccount", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-tai-khoan-giao-vien", defaults: new { controller = "TeacherAccount", action = "PostTeacherAccount", area = "Home" });
            context.MapRoute("", "xoa-tai-khoan-giao-vien", defaults: new { controller = "TeacherAccount", action = "DeleteTeacherAccount", area = "Home", id = UrlParameter.Optional });
            #endregion
            #region lớp
            context.MapRoute("", "lop", defaults: new { controller = "Class", action = "Classes", area = "Home" });
            context.MapRoute("", "danh-sach-lop", defaults: new { controller = "Class", action = "GetPage_Classes", area = "Home" });
            context.MapRoute("", "chi-tiet-lop/{id}", defaults: new { controller = "Class", action = "Class", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-lop", defaults: new { controller = "Class", action = "Class", area = "Home" });
            context.MapRoute("", "xoa-lop", defaults: new { controller = "Class", action = "DeleteClass", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "chi-tiet-lich-hoc-cua-lop/{id}", defaults: new { controller = "ClassWeekDay", action = "Class", area = "Home", id = UrlParameter.Optional });

            #endregion

            #region học sinh thuộc lop
            context.MapRoute("", "danh-sach-hoc-sinh-theo-lop/{classID}", defaults: new { controller = "Class", action = "GetPage_StudenByClassID", area = "Home", classID = UrlParameter.Optional });
            context.MapRoute("", "chi-tiet-hoc-sinh-theo-lop/{id}", defaults: new { controller = "Class", action = "ClassStudent", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-hoc-sinh-theo-lop", defaults: new { controller = "Class", action = "ClassStudent", area = "Home" });
            context.MapRoute("", "xoa-hoc-sinh-theo-lop", defaults: new { controller = "Class", action = "DeleteClassStudent", area = "Home", id = UrlParameter.Optional });
            #endregion
            #region học phí
            context.MapRoute("", "hoc-phi", defaults: new { controller = "Payment", action = "Payments", area = "Home" });
            context.MapRoute("", "danh-sach-hoc-phi", defaults: new { controller = "Payment", action = "GetPage_Payments", area = "Home" });
            context.MapRoute("", "chi-tiet-hoc-phi/{id}", defaults: new { controller = "Payment", action = "Payment", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-hoc-phi", defaults: new { controller = "Payment", action = "PostPayment", area = "Home" });
            context.MapRoute("", "xoa-hoc-phi", defaults: new { controller = "Payment", action = "DeletePayment", area = "Home", id = UrlParameter.Optional });
            #endregion
            #region tin tức
            context.MapRoute("", "tin-tuc", defaults: new { controller = "NewsFeed", action = "NewsFeeds", area = "Home" });
            context.MapRoute("", "ban-tin", defaults: new { controller = "ViewNewsFed", action = "Index", area = "Home", slug = UrlParameter.Optional });
            context.MapRoute("", "danh-sach-tin-tuc", defaults: new { controller = "NewsFeed", action = "GetPage_NewsFeeds", area = "Home" });
            context.MapRoute("", "chi-tiet-tin-tuc/{id}", defaults: new { controller = "NewsFeed", action = "NewsFeed", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-tin-tuc", defaults: new { controller = "NewsFeed", action = "PostNewsFeed", area = "Home" });
            context.MapRoute("", "xoa-tin-tuc", defaults: new { controller = "NewsFeed", action = "DeleteNewsFeed", area = "Home", id = UrlParameter.Optional });

            #endregion
            #region điểm của lớp
            context.MapRoute("", "diem-lop", defaults: new { controller = "TrainingResult", action = "TrainingResults", area = "Home" });
            context.MapRoute("", "danh-sach-diem-lop", defaults: new { controller = "TrainingResult", action = "GetPage_TrainingResults", area = "Home" });
            context.MapRoute("", "danh-sach-chi-tiet-diem-lop/{id}", defaults: new { controller = "TrainingResult", action = "GetPage_TrainingResultDetails", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "chi-tiet-diem-lop/{id}", defaults: new { controller = "TrainingResult", action = "TrainingResult", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-diem-lop", defaults: new { controller = "TrainingResult", action = "PostTrainingResult", area = "Home" });
            context.MapRoute("", "xoa-diem-lop", defaults: new { controller = "TrainingResult", action = "DeleteTrainingResult", area = "Home", id = UrlParameter.Optional });
            #endregion
            #region chi tiết  điểm của lớp
            context.MapRoute("", "chi-tiet-diem-lop", defaults: new { controller = "TrainingResultDetail", action = "TrainingResultDetails", area = "Home" });
            context.MapRoute("", "danh-sach-chi-tiet-diem-lop/{id}", defaults: new { controller = "TrainingResultDetail", action = "GetPage_TrainingResultDetails", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "chi-tiet-diem-hoc-sinh/{id}", defaults: new { controller = "TrainingResult", action = "TrainingResultDetail", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "them-moi-chi-tiet-diem-hoc-sinh/{id}", defaults: new { controller = "TrainingResult", action = "InsertTrainingResultDetail", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-diem-hoc-sinh", defaults: new { controller = "TrainingResultDetail", action = "PostTrainingResultDetail", area = "Home" });
            context.MapRoute("", "xoa-diem-hoc-sinh", defaults: new { controller = "TrainingResultDetail", action = "DeleteTrainingResultDetail", area = "Home", id = UrlParameter.Optional });
            #endregion

            #region lịch học theo lớp
            context.MapRoute("", "danh-lich-hoc-theo-lop/{classID}", defaults: new { controller = "ClassWeekDay", action = "Get_ClassWeekDayByClassID", area = "Home", classID = UrlParameter.Optional });
            context.MapRoute("", "chi-tiet-lich-hoc-theo-lop/{id}", defaults: new { controller = "ClassWeekDay", action = "ClassWeekDay", area = "Home", id = UrlParameter.Optional });
            context.MapRoute("", "luu-hoc-lich-hoc-lop", defaults: new { controller = "ClassWeekDay", action = "ClassWeekDay", area = "Home" });
            context.MapRoute("", "xoa-hoc-lich-hoc-lop", defaults: new { controller = "ClassWeekDay", action = "DeleteClassClassWeekDay", area = "Home", id = UrlParameter.Optional });
            #endregion



            
        }
    }
}