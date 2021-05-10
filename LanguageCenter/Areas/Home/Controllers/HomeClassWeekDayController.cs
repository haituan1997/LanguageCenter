using LanguageCenter.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanguageCenter.Areas.Home.Models.Class;
using AutoMapper;
using LanguageCenter.Layer.DataLayer.Object;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class HomeClassWeekDayController : Controller
    {
        private readonly ClassWeekDayRepository _classWeekDayRepository;
        private readonly ClassRepository _classRepository;
        public HomeClassWeekDayController() 
        {
            _classWeekDayRepository = new ClassWeekDayRepository();
            _classRepository = new ClassRepository();
            Mapper.CreateMap<Class, ClassModel>();
        }
        // GET: Home/HomeClassWeekDay
        public ActionResult ClassWeekDay(int? index)
        {
            index = index == null ? 1 : index;
            var datas = _classRepository.Get_AllClassesByIndex(index).ToList();
            ViewBag.Classes = datas;
            return View("ClassWeekDay");
        }
        public ActionResult FullCalendar(long? classID)
        {
            int total;
            if (classID != null)
            {
                var data = _classWeekDayRepository.Get_ClassWeekDayByClassID((int)classID, out total, 1, 999999, "", "");
            }
            var model = Mapper.Map<Class, ClassModel>(  _classRepository.Get_ClassByClassID((long)classID));
            return View("FullCalendar", model);
        }
        public ActionResult GetCalendarByClassID(long? classID)
        {
            int total = 0;
            var datas = new List<ClassWeekDay>();
            if (classID != null)
            {
                datas = _classWeekDayRepository.Get_ClassWeekDayByClassID((int)classID, out total, 1, 999999, "", "").ToList();
            }
            return Json(new { success = true, data = datas }, JsonRequestBehavior.AllowGet);
        }
    }
}