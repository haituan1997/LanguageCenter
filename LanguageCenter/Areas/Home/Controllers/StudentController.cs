using LanguageCenter.Areas.Home.Models.StudentModel;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DataTables.Mvc;
using LanguageCenter.Code.Helper.DatatableHelper;
using LanguageCenter.Helper;

namespace LanguageCenter.Areas.Home.Controllers
{
    [CustomAuthorize("3")]
    public class StudentController : Controller
    {
        private readonly StudentRepository _StudentRepository;
        public StudentController() {
            _StudentRepository = new StudentRepository();
            Mapper.CreateMap<Student, StudentModel>();
            Mapper.CreateMap<StudentModel, Student>();
        }
        // GET: Home/Student
        public ActionResult Students()
        {
            return View();
            
        }

        [HttpPost]
        public ActionResult GetPage_Students([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _StudentRepository.Get_Students(out totalRows, pageIndex,pageSize,orderBy,searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Student(long? id)
        {
            if (id == null)
            {
                
                var model = new StudentModel();
                model.DateOfBirth = DateTime.Now;
                model.Title = "Thêm mới sinh viên";
                model.IsEdit = false;
                return PartialView("_StudentPopup", model);
            }
            else
            {
                var Student = _StudentRepository.Get_StudentByStudentID((long)id);
                var model = Mapper.Map<Student, StudentModel>(Student);
                model.Title = "Cập nhập sinh viên";
                model.IsEdit = true;
                return PartialView("_StudentPopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostStudent")]
        public ActionResult PostStudent(StudentModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                if (model.IsEdit == true)
                {
                    var Student = Mapper.Map<StudentModel, Student>(model);
                    _StudentRepository.Update(Student);

                    return Json(new { success = true, message = "Cập nhập sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Student = Mapper.Map<StudentModel, Student>(model);
                    _StudentRepository.Insert(Student);

                    return Json(new { success = true, message = "Thêm mới sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
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
                _StudentRepository.Delete(id);
                var message = "Xóa sinh viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sinh viên đã được dùng ở chức năng khác" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}