using AutoMapper;
using LanguageCenter.Areas.Home.Models.TeacherModel;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{

    public class HomeController : Controller
    {

        // GET: Home/Home
        private readonly ClassRepository _ClassRepository;
        private readonly StudentRepository _studentRepository;
        private readonly TeacherRepository _teacherRepository;
        private readonly CourseRepository _courseRepository;
        public HomeController()
        {
            _ClassRepository = new ClassRepository();
            _teacherRepository = new TeacherRepository();
            _courseRepository = new CourseRepository();
            _studentRepository = new StudentRepository();
            Mapper.CreateMap<Teacher, TeacherModel>();
        }
        public ActionResult Index()
        {  
            if(!string.IsNullOrEmpty(User.Identity.Name))
            {
                ViewBag.User = User.Identity.Name;
                ViewBag.UserID = ((ClaimsIdentity)User.Identity).FindFirst("UserID").Value;
            }    
            
            return View();
        }

        [ChildActionOnly]
        public ActionResult AllData()
        {
            var data = new List<int>();
            var student = _studentRepository.GetAll_Students().Count();
            data.Add(student);

            var teacher = _teacherRepository.Get_Teacheres().Count();
            data.Add(teacher);

            var course = _courseRepository.Get_AllCourses().Count();
            data.Add(course);

            var objclass = _ClassRepository.Get_AllClasses().Count();
            data.Add(objclass);
            ViewBag.AllData = data;
            return PartialView("_AllData");
        }
        [ChildActionOnly]
        public ActionResult Teachers()
        {
            var teachers = _teacherRepository.Get_Teacheres();
            ViewBag.DataTeachers = teachers;
            return PartialView("_Teachers");
        }

    }
}