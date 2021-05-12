using AutoMapper;
using DataTables.Mvc;
using LanguageCenter.Areas.Home.Models.Class;
using LanguageCenter.Areas.Home.Models.ClassWeekDay;
using LanguageCenter.Code.Helper.DatatableHelper;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.Helper;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{
    [CustomAuthorize("3")]
    public class ClassWeekDayController : Controller
    {
        private static ClassRepository _classRepository;
        private static ClassWeekDayRepository _classWeekDayRepository;
        private readonly TeacherRepository _teacherRepository;
        private readonly CourseRepository _courseRepository;
        private readonly ClassStudentRepository _classStudentRepository;
        public ClassWeekDayController() 
        {
            _classRepository = new ClassRepository();
            _classWeekDayRepository = new ClassWeekDayRepository();
            _teacherRepository = new TeacherRepository();
            _courseRepository = new CourseRepository();
            _classStudentRepository = new ClassStudentRepository();
            Mapper.CreateMap<ClassWeekDayModel, ClassWeekDay>();
            Mapper.CreateMap<Class, NewClassModel>();
            Mapper.CreateMap<ClassWeekDay, ClassWeekDayModel>();
        }
        // GET: Home/ClassWeekDay
        public ActionResult Class(long? id)
        {
            ViewBag.Courses = _courseRepository.Get_AllCourses().ToList();
            ViewBag.Teachers = _teacherRepository.Get_AllTeacheres().ToList();
            
                var obj = _classRepository.Get_ClassByClassID((long)id);
                var model = Mapper.Map<Class, NewClassModel>(obj);
                model.Title = "Cập nhập lịch học của lớp";
                model.IsEdit = true;
                ViewBag.ClassID = model.ClassID;
                return View("ClassWeekDays", model);
        }
        [HttpPost]
        public ActionResult Get_ClassWeekDayByClassID([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, long? classID)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;
            if (searchBy.Contains("FullName"))
            {
                searchBy = searchBy.Replace("FullName", "LastName");
            }
            if (orderBy.Contains("FullName"))
            {
                orderBy = orderBy.Replace("FullName", "LastName");
            }
            var datas = _classWeekDayRepository.Get_ClassWeekDayByClassID(classID, out totalRows, pageIndex, pageSize, orderBy, searchBy);
            
            var culture = new System.Globalization.CultureInfo("vi-vn");
            foreach (var item in datas)
            {
                item.ClassWeekDayName = culture.DateTimeFormat.GetDayName(item.ClassWeekDayTime.DayOfWeek);
            }
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = datas.ToArray() }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ClassWeekDay(long? id)
        {
            var model = new ClassWeekDayModel();
            if (id == null)
            {
                model.Title = "Thêm mới lịch học";
                model.IsEdit = false;
                return PartialView("_ClassWeekDay", model);
            }
            else
            {
                var obj = _classWeekDayRepository.Get_ClassWeekDayByID((long)id);
                model = Mapper.Map<ClassWeekDay, ClassWeekDayModel>(obj);
                model.Title = "Cập nhập lịch học";
                model.IsEdit = true;
                return PartialView("_ClassWeekDay", model);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("ClassWeekDay")]
        public ActionResult PostClassStudent(ClassWeekDayModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra!" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (model.IsEdit)
                {
                    var obj = Mapper.Map<ClassWeekDayModel, ClassWeekDay>(model);
                    _classWeekDayRepository.Updaet(obj);
                    return Json(new { success = true, message = "Cập nhập lịch học cho lớp thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var obj = Mapper.Map<ClassWeekDayModel, ClassWeekDay>(model);
                    _classWeekDayRepository.Insert(obj);
                    return Json(new { success = true, message = "Cập nhập lịch học cho lớp thành công!" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [ActionName("DeleteClassClassWeekDay")]
        public ActionResult DeleteClassClassWeekDay(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _classWeekDayRepository.Delete(id);
                var message = "Xóa học sinh khỏi lớp thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}