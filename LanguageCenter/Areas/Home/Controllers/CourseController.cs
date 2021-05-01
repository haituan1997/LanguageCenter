using AutoMapper;
using DataTables.Mvc;
using LanguageCenter.Areas.Home.Models.Course;
using LanguageCenter.Areas.Home.Models.StudentModel;
using LanguageCenter.Code.Helper.DatatableHelper;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseRepository _courseRepository;
        public CourseController()
        {
            _courseRepository = new CourseRepository();
            Mapper.CreateMap<Course, CourseModel>();
            Mapper.CreateMap<CourseModel, Course>();
        }
        // GET: Home/Student
        public ActionResult Courses()
        {
            return View();

        }

        [HttpPost]
        public ActionResult GetPage_Courses([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _courseRepository.Get_Courses(out totalRows, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Course(long? id)
        {
            if (id == null)
            {
                var model = new CourseModel();
                model.Title = "Thêm mới khóa đào tạo";
                model.IsEdit = false;
                return PartialView("_CoursePopup", model);
            }
            else
            {
                var course = _courseRepository.Get_CourseByCourseID((long)id);
                var model = Mapper.Map<Course, CourseModel>(course);
                model.Title = "Cập nhập khóa đào tạo";
                model.IsEdit = true;
                return PartialView("_CoursePopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostCourse")]
        public ActionResult PostCourse(CourseModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                if (model.IsEdit == true)
                {
                    var course = Mapper.Map<CourseModel, Course>(model);
                    _courseRepository.Update(course);
                    return Json(new { success = true, message = "Cập nhập khóa đào tạo thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var course = Mapper.Map<CourseModel, Course>(model);
                    _courseRepository.Insert(course);
                    return Json(new { success = true, message = "Thêm mới khóa đào tạo thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeleteStudent")]
        public ActionResult DeleteStudent(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _courseRepository.Delete(id);
                var message = "Xóa khóa đào tạo thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}