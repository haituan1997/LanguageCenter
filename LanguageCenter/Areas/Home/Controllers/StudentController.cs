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

namespace LanguageCenter.Areas.Home.Controllers
{
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
                model.Title = "Thêm mới học sinh";
                model.IsEdit = false;
                return PartialView("_StudentPopup", model);
            }
            else
            {
                var Student = _StudentRepository.Get_StudentByStudentID((long)id);
                var model = Mapper.Map<Student, StudentModel>(Student);
                model.Title = "Cập nhập học sinh";
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
                    Student.DateOfBirth = DateTime.ParseExact(model.DateOfBirthBackUp, "dd/MM/yyyy", null);
                    _StudentRepository.Update(Student);

                    return Json(new { success = true, message = "Cập nhập học sinh thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Student = Mapper.Map<StudentModel, Student>(model);
                    Student.DateOfBirth = DateTime.ParseExact(model.DateOfBirthBackUp, "dd/MM/yyyy", null);
                    _StudentRepository.Insert(Student);

                    return Json(new { success = true, message = "Thêm mới học sinh thành công!" }, JsonRequestBehavior.AllowGet);
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
                var message = "Xóa học sinh thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}