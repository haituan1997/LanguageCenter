using AutoMapper;
using LanguageCenter.Areas.Home.Models.Class;
using LanguageCenter.Areas.Home.Models.Course;
using LanguageCenter.Areas.Home.Models.TeacherModel;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.Helper;
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
   
    public class HomeController : BaseController
    {

        // GET: Home/Home
        private readonly ClassRepository _ClassRepository;
        private readonly StudentRepository _studentRepository;
        private readonly TeacherRepository _teacherRepository;
        private readonly CourseRepository _courseRepository;
        private readonly NewsFeedRepository _NewsFeedRepository;
        private readonly RegistrationClassRepository _registrationClassRepository;
        public HomeController()
        {
            _ClassRepository = new ClassRepository();
            _teacherRepository = new TeacherRepository();
            _courseRepository = new CourseRepository();
            _studentRepository = new StudentRepository();
            _NewsFeedRepository = new NewsFeedRepository();
            _registrationClassRepository = new RegistrationClassRepository();
            Mapper.CreateMap<Teacher, TeacherModel>();
            Mapper.CreateMap<Class, ClassModel>();
            Mapper.CreateMap<Course, CourseModel>();
        }
        public ActionResult Index()
        {  
           
            int total = 0;
            var newsfed = _NewsFeedRepository.Get_NewsFeeds(out total, 1, 10, null, null).ToList();
            foreach(var item in newsfed)
            {
                item.Thumb = Getthum(item.Description);
            }
            var fisrtNews = new NewsFeed();
            if (newsfed.Count()>0)
            {
                fisrtNews = newsfed[0];
            }
            var listNews = new List<NewsFeed>();
            if (newsfed.Count>0)
            {
                newsfed.RemoveAt(0);

            }
            ViewBag.newFeds = newsfed;
            ViewBag.firstNewFeds = fisrtNews;
            ViewBag.Total = total;
            return View();
        }
        public string Getthum(string value)
        {
            if(value.IndexOf("<img") >=0)
            {
                value=value.Substring(value.IndexOf("<img"), value.Length - value.IndexOf("<img"));
                value=value.Substring(value.IndexOf("src=")+5, value.Length - value.IndexOf("src=")-5);
                value = value.Substring(0, value.IndexOf('"'));
            }    
            return value;
        }
        [ChildActionOnly]
        public ActionResult AllData()
        {
            var data = new List<int>();
            var student = _studentRepository.GetAll_Students().Count();
            data.Add(student);

            var teacher = _teacherRepository.Get_AllTeacheres().Count();
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
            var teachers = _teacherRepository.Get_AllTeacheres();
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
            ViewBag.CourseID = id;
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
            ViewBag.StudentID = StudentID;
            if (id != null)
            {
                indexNumber = indexNumber != null ? indexNumber : 1;
                var classes = Mapper.Map<IEnumerable<Class>, IEnumerable<ClassModel>>(_ClassRepository.Get_Class_ByCourseID((long)id, (int)indexNumber));
                if (StudentID != -1)
                {
                    var listClassesRegistered = _registrationClassRepository.GetByStudentId(StudentID);
                    foreach (var item in classes)
                    {
                        if (listClassesRegistered.Any(x=>x.ClassID == item.ClassID))
                            item.IsRegistered = true;
                    }
                }
                ViewBag.Class = (classes);
                ViewBag.ToTalCount = classes.Count();
            }
            else
            {
                indexNumber = 1;
                var classes = courses != null ? Mapper.Map<IEnumerable<Class>, IEnumerable<ClassModel>>(_ClassRepository.Get_Class_ByCourseID(courses.FirstOrDefault().CourseID, (int)indexNumber)) : null;
                if (StudentID != -1)
                {
                    var listClassesRegistered = _registrationClassRepository.GetByStudentId(StudentID);
                    foreach (var item in classes)
                    {
                        if (listClassesRegistered.Any(x => x.ClassID == item.ClassID))
                            item.IsRegistered = true;
                    }
                }
                ViewBag.Class = classes;
                if (courses != null)
                {
                    ViewBag.ToTalCount = _ClassRepository.Get_Class_ByCourseID(courses.FirstOrDefault().CourseID, (int)indexNumber).Count();
                }
            }
            return PartialView("_ClassByCourseID");
        }



        #region lịch học

        public ActionResult ClassWeekDay()
        {
            return View();
        }
        #endregion

    }
}