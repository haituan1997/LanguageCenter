using LanguageCenter.Areas.Home.Models.StudentAccountModel;
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
    public class StudentAccountController : Controller
    {
        private readonly StudentAccountRepository _StudentAccountRepository;
        private readonly StudentRepository _StudentRepository;
        public StudentAccountController() {
            _StudentAccountRepository = new StudentAccountRepository();
            _StudentRepository = new StudentRepository();
            Mapper.CreateMap<StudentAccount, StudentAccountModel>();
            Mapper.CreateMap<StudentAccountModel, StudentAccount>();
        }
        // GET: Home/StudentAccount
        public ActionResult StudentAccounts()
        {
            return View();
            
        }

        [HttpPost]
        public ActionResult GetPage_StudentAccounts([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _StudentAccountRepository.Get_StudentAccounts(out totalRows, pageIndex,pageSize,orderBy,searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult StudentAccount(long? id)
        {
            ViewBag.Students = _StudentRepository.GetAll_StudentsNotAccont().ToList();
            if (id == null)
            {
                
                var model = new StudentAccountModel();
                model.Title = "Thêm mới tài khoản sinh viên";
                model.IsEdit = false;
                return PartialView("_StudentAccountPopup", model);
            }
            else
            {
                var StudentAccount = _StudentAccountRepository.Get_StudentAccountByStudentAccountID((long)id);
                var model = Mapper.Map<StudentAccount, StudentAccountModel>(StudentAccount);
                model.Title = "Cập nhập tài khoản sinh viên";
                model.IsEdit = true;
                return PartialView("_StudentAccountPopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostStudentAccount")]
        public ActionResult PostStudentAccount(StudentAccountModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                if (model.IsEdit == true)
                {
                    var StudentAccount = Mapper.Map<StudentAccountModel, StudentAccount>(model);
                    _StudentAccountRepository.Update(StudentAccount);

                    return Json(new { success = true, message = "Cập nhập tài khoản sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var checkUserName = _StudentAccountRepository.Get_StudentAccountByUserName(model.UserLogin);
                    if(checkUserName== null)
                    {
                        var StudentAccount = Mapper.Map<StudentAccountModel, StudentAccount>(model);
                        _StudentAccountRepository.Insert(StudentAccount);

                        return Json(new { success = true, message = "Thêm mới tài khoản sinh viên thành công!" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                        return Json(new { success = false, Message="tên đăng nhập đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeleteStudentAccount")]
        public ActionResult DeleteStudentAccount(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _StudentAccountRepository.Delete(id);
                var message = "Xóa tài khoản sinh viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}