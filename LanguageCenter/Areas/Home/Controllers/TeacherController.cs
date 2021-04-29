using LanguageCenter.Areas.Home.Models.TeacherModel;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class TeacherController : Controller
    {
        private readonly TeacherRepository _teacherRepository;
        public TeacherController() {
            _teacherRepository = new TeacherRepository();
            Mapper.CreateMap<Teacher, TeacherModel>();
        }
        // GET: Home/Teacher
        public ActionResult Teacheres()
        {
            return View();
            
        }

        [HttpGet]
        public ActionResult GetTeacheres()
        {
            var teacheres = _teacherRepository.Get_Teacheres();
            return Json(new { data = teacheres }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Teacher(long? id)
        {
            var teacher = new Teacher();
            if (id == null)
            {
                var model = Mapper.Map <Teacher, TeacherModel>(teacher);
                return PartialView("_TeacherPopup", model);
            }
            else
            {
                teacher = _teacherRepository.Get_Teacheres().FirstOrDefault(x => x.TeacherID == (long)id);
                var model = Mapper.Map<Teacher, TeacherModel>(teacher);
                model.Title = "Thêm mới giáo viên";
                return PartialView("_TeacherPopup", model);
            }
        }
    }
}