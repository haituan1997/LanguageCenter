using AutoMapper;
using LanguageCenter.Areas.Home.Models.Class;
using LanguageCenter.Areas.Home.Models.Course;
using LanguageCenter.Areas.Home.Models.TeacherModel;
using LanguageCenter.DataLayer.Object;
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
            Mapper.CreateMap<Class, ClassModel>();
            Mapper.CreateMap<Course, CourseModel>();
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


        [ChildActionOnly]
        public ActionResult Courses()//Home menu
        {
            var courses = _courseRepository.Get_AllCourses().Take(5).ToList();
            ViewBag.DataTeachers = courses;
            return PartialView("_Courses");
        }


        public ActionResult Course(long? id,int? indexNumber)
        {
            var courses = _courseRepository.Get_AllCourses().ToList();
            ViewBag.Course = Mapper.Map<IEnumerable<Course>, IEnumerable<CourseModel>>(courses);
            ViewBag.Class = null;
            ViewBag.ToTalCount = null;
            if (id != null)
            {
                indexNumber = indexNumber != null ? indexNumber : 1;
                ViewBag.Class = Mapper.Map<IEnumerable<Class>,IEnumerable< ClassModel>>(_ClassRepository.Get_Class_ByCourseID((long)id, (int)indexNumber));
                ViewBag.ToTalCount = _ClassRepository.Get_Class_ByCourseID((long)id, (int)indexNumber).Count();
            }
            else
            {
                indexNumber = 1;
                ViewBag.Class = courses != null ? Mapper.Map<IEnumerable<Class>, IEnumerable<ClassModel>>(_ClassRepository.Get_Class_ByCourseID(courses.FirstOrDefault().CourseID, (int)indexNumber)) : null ;
                if (courses != null)
                {
                    ViewBag.ToTalCount = _ClassRepository.Get_Class_ByCourseID(courses.FirstOrDefault().CourseID, (int)indexNumber).Count();
                }
            }
            return View("Course");
        }


        public ActionResult CourseMenuRight()
        {
            var courses = _courseRepository.Get_CoursesAndCountClass().Take(15).ToList();
            ViewBag.CoursesMenuRught = courses;
            return PartialView("_CourseMenuRight");
        }


        public ActionResult ClassByCourseID(long? id, int? indexNumber)
        {
            var courses = _courseRepository.Get_AllCourses().ToList();
            ViewBag.Class = null;
            if (id != null)
            {
                indexNumber = indexNumber != null ? indexNumber : 1;
                ViewBag.Class = Mapper.Map<IEnumerable<Class>, IEnumerable<ClassModel>>(_ClassRepository.Get_Class_ByCourseID((long)id, (int)indexNumber));
                ViewBag.ToTalCount = _ClassRepository.Get_Class_ByCourseID((long)id, (int)indexNumber).Count();
            }
            else
            {
                indexNumber = 1;
                ViewBag.Class = courses != null ? Mapper.Map<IEnumerable<Class>, IEnumerable<ClassModel>>(_ClassRepository.Get_Class_ByCourseID(courses.FirstOrDefault().CourseID, (int)indexNumber)) : null;
                if (courses != null)
                {
                    ViewBag.ToTalCount = _ClassRepository.Get_Class_ByCourseID(courses.FirstOrDefault().CourseID, (int)indexNumber).Count();
                }
            }
            return PartialView("_ClassByCourseID");
        }

    }
}