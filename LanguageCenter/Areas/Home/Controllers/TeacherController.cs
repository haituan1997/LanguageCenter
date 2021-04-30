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
            Mapper.CreateMap<TeacherModel,Teacher >();
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
                model.Title = "Thêm mới giáo viên";
                model.IsEdit = false;
                return PartialView("_TeacherPopup", model);
            }
            else
            {
                teacher = _teacherRepository.Get_Teacheres().FirstOrDefault(x => x.TeacherID == (long)id);
                var model = Mapper.Map<Teacher, TeacherModel>(teacher);
                model.Title = "Cập nhập giáo viên";
                model.IsEdit = true;
                return PartialView("_TeacherPopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostTeacher")]
        public ActionResult PostTeacher(TeacherModel model)
        {
            if (!ModelState.IsValid)
                 throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                var teacher = Mapper.Map<TeacherModel, Teacher>(model);
                _teacherRepository.InsertOrUpdate(teacher);
                var message = model.IsEdit == true ? "Cập nhập giáo viên thành công!" : "Thêm mới giáo viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeleteTeacher")]
        public ActionResult DeleteTeacher(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!"}, JsonRequestBehavior.AllowGet);
            try
            {
                _teacherRepository.Delete(id);
                var message = "Xóa giáo viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}