using AutoMapper;
using DataTables.Mvc;
using LanguageCenter.Areas.Home.Models.Class;
using LanguageCenter.Areas.Home.Models.ClassStudent;
using LanguageCenter.Areas.Home.Models.TeacherModel;
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
using static Code.Enumerator.Enumerator;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class ClassController : BaseController
    {
        private readonly TeacherRepository _teacherRepository;
        private readonly ClassRepository _classRepository;
        private readonly CourseRepository _courseRepository;
        private readonly ClassStudentRepository _classStudentRepository;
        public ClassController()
        {
            _teacherRepository = new TeacherRepository();
            _classRepository = new ClassRepository();
            _courseRepository = new CourseRepository();
            _classStudentRepository = new ClassStudentRepository();
            Mapper.CreateMap<Teacher, TeacherModel>();
            Mapper.CreateMap<Class, NewClassModel>();
            Mapper.CreateMap<Class, ClassModel>();
            Mapper.CreateMap<NewClassModel, Class>();
            Mapper.CreateMap<ClassStudent, ClassStudentModel>();
            Mapper.CreateMap<ClassStudentModel, ClassStudent>();

            
        }

        // GET: Home/Class
        public ActionResult Classes()
        {
            if (TeacherID != -1)
                ViewBag.Function = new Tuple<int, int>((int)TypeOfPermission.Type0, (int)TypeOfPermission.Type1);
            return View();
        }
        [HttpPost]
        public ActionResult GetPage_Classes([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
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
            if (searchBy.Contains("CourseName"))
            {
                searchBy = searchBy.Replace("CourseName", "Name");
            }
            if (orderBy.Contains("CourseName"))
            {
                orderBy = orderBy.Replace("CourseName", "Name");
            }

            if (TeacherID != -1)
                searchBy += " AND cl.TeacherID =" + TeacherID;

            var data = _classRepository.Get_Classes(out totalRows, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult Class(long? id)
        {
            ViewBag.Courses = _courseRepository.Get_AllCourses().ToList();
            ViewBag.Teachers = _teacherRepository.Get_AllTeacheres().ToList();
            if (id == null)
            {
                var model = new Class
                {
                    Title = "Thêm mới lớp học",
                    IsCreated = false,
                    //TeacherID = 0,
                    //CourseID = 0,
                };
                var newModel = new NewClassModel
                {
                    ClassID = _classRepository.Insert(model),
                    Title = model.Title,
                    IsCreated = model.IsCreated,
                };
                ViewBag.ClassID = newModel.ClassID;
                return View("Class", newModel);
            }
            else
            {
                var obj = _classRepository.Get_ClassByClassID((long)id);
                var model = Mapper.Map<Class, NewClassModel>(obj);
                model.Title = "Cập nhập lớp học";
                model.IsEdit = true;
                ViewBag.ClassID = model.ClassID;
                return View("Class", model);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("Class")]
        public ActionResult Class(NewClassModel model)
        {
            ViewBag.Courses = _courseRepository.Get_AllCourses().ToList();
            ViewBag.Teachers = _teacherRepository.Get_AllTeacheres().ToList();
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Đã có lỗi xảy ra";
                return View("Class", model);
            }
            try
            {
                if (model.IsCreated)
                {
                    var objClass = Mapper.Map<NewClassModel, Class>(model);
                    _classRepository.Update(objClass);
                    TempData["Success"] = "Cập nhập lớp thành công!";
                }
                else
                {
                    var objClass = Mapper.Map<NewClassModel, Class>(model);
                    objClass.IsCreated = true;
                    _classRepository.Update(objClass);
                    TempData["Success"] = "Thêm mới lớp thành công!";
                }

                return RedirectToAction("Classes", "Class", new { id = "" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("Class", model);
            }
        }
        [ActionName("DeleteClass")]
        public ActionResult DeleteClass(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _classRepository.Delete(id);
                var message = "Xóa lớp học thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetPage_StudenByClassID([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, long? classID)
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
            var data = _classStudentRepository.Get_ClassStudentByClassID(classID, out totalRows, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ClassStudent(long id)
        {
            ViewBag.StudentNotIntClass = _classStudentRepository.Get_StudentNotInClass(id);
            var model = new ClassStudentModel();
            model.Title = "Chọn học sinh";
            model.ClassID = id;
            return PartialView("_ClassStudent", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("ClassStudent")]
        public ActionResult PostClassStudent(ClassStudent model)
        {
            ViewBag.StudentNotIntClass = _classStudentRepository.Get_StudentNotInClass(model.ClassID);

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra!" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                _classStudentRepository.Insert(model);
                return Json(new { success = true, message = "Thêm mới học sinh vào lớp thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [ActionName("DeleteClassStudent")]
        public ActionResult DeleteClassStudent(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _classStudentRepository.Delete(id);
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