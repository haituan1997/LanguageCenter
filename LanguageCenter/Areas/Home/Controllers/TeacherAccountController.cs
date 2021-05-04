using AutoMapper;
using DataTables.Mvc;
using LanguageCenter.Areas.Home.Models.TeacherAccountModel;
using LanguageCenter.Areas.Home.Models.TeacherModel;
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
    public class TeacherAccountController : Controller
    {
        private readonly TeacherAccountRepository _teacherAccountRepository;
        private readonly TeacherRepository _teacherRepository;
        public TeacherAccountController()
        {
            _teacherAccountRepository = new TeacherAccountRepository();
            _teacherRepository = new TeacherRepository();
            Mapper.CreateMap<Teacher, TeacherModel>();
            Mapper.CreateMap<TeacherAccount, TeacherAccountModel>();
            Mapper.CreateMap<TeacherAccountModel, TeacherAccount>();
        }
        // GET: Home/StudentAccount
        public ActionResult TeacherAccounts()
        {
            return View();

        }
        // GET: Home/TeacherAccount
        [HttpPost]
        public ActionResult GetPage_TeacherAccounts([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
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
            var data = _teacherAccountRepository.Get_TeacherAccounts(out totalRows, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult TeacherAccount(long? id)
        {
            ViewBag.Teachers = _teacherRepository.Get_TeacheresNotInTeacherAccount().ToList();
            if (id == null)
            {

                var model = new TeacherAccountModel();
                model.Title = "Thêm mới tài khoản giáo viên";
                model.IsEdit = false;
                model.UserLogin = "GV-";
                return PartialView("_TeacherAccountPopup", model);
            }
            else
            {
                var teacherAccount = _teacherAccountRepository.Get_StudentAccountByStudentAccountID((long)id);
                var model = Mapper.Map<TeacherAccount, TeacherAccountModel>(teacherAccount);
                model.Title = "Cập nhập tài khoản giáo viên";
                model.IsEdit = true;
                return PartialView("_TeacherAccountPopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostTeacherAccount")]
        public ActionResult PostTeacherAccount(TeacherAccountModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                if (model.IsEdit == true)
                {
                    var teacherAccount = Mapper.Map<TeacherAccountModel, TeacherAccount>(model);
                    _teacherAccountRepository.Update(teacherAccount);

                    return Json(new { success = true, message = "Cập nhập tài khoản giáo viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var teacherAccount = Mapper.Map<TeacherAccountModel, TeacherAccount>(model);
                    _teacherAccountRepository.Insert(teacherAccount);
                    return Json(new { success = true, message = "Thêm mới tài khoản giáo viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ActionName("DeleteTeacherAccount")]
        public ActionResult DeleteTeacherAccountModel(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _teacherAccountRepository.Delete(id);
                var message = "Xóa tài khoản học sinh thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}