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
        }
        // GET: Home/Student
        public ActionResult Students()
        {
            return View();
            
        }

        [HttpPost]
        public ActionResult GetStudents([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
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
            var Student = new Student();
            if (id == null)
            {
                var model = Mapper.Map <Student, StudentModel>(Student);
                model.Title = "Thêm mới giáo viên";
                model.IsEdit = false;
                return PartialView("_StudentPopup", model);
            }
            else
            {
                Student = new Student();
                var model = Mapper.Map<Student, StudentModel>(Student);
                model.Title = "Cập nhập giáo viên";
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
                    return Json(new { success = true, message = "Cập nhập giáo viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Student = Mapper.Map<StudentModel, Student>(model);
                    return Json(new { success = true, message = "Thêm mới giáo viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}