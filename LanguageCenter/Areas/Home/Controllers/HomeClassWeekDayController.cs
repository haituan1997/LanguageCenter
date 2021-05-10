using LanguageCenter.Helper;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{
    [CustomAuthorize("3")]
    public class HomeClassWeekDayController : Controller
    {
        private readonly ClassWeekDayRepository _classWeekDayRepository;
        private readonly ClassRepository _classRepository;
        public HomeClassWeekDayController() 
        {
            _classWeekDayRepository = new ClassWeekDayRepository();
            _classRepository = new ClassRepository();
        }
        // GET: Home/HomeClassWeekDay
        public ActionResult ClassWeekDay(int? index)
        {
            index = index == null ? 1 : index;
            var datas = _classRepository.Get_AllClassesByIndex(index).ToList();
            ViewBag.Classes = datas;
            return View("ClassWeekDay");
        }
        public ActionResult FullCalendar(int? classID)
        {
            //var classes = _classRepository.Get_AllClasses();
            //int total;
            //if (classID != null)
            //{
            //    var data = _classWeekDayRepository.Get_ClassWeekDayByClassID((int)classID, out total, 1, 999999, "", "");
            //}
            ViewBag.ClassID = classID;
            return View("FullCalendar");
        }
    }
}