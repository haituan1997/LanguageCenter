using LanguageCenter.Areas.Home.Models.PaymentModel;
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
using Code.Helper.StaticData;
using LanguageCenter.Helper;

namespace LanguageCenter.Areas.Home.Controllers
{
    [CustomAuthorize("3")]
    public class PaymentController : Controller
    {
        private readonly PaymentRepository _PaymentRepository;
        private readonly ClassRepository _ClassRepository;
        private readonly ClassStudentRepository _ClassStudentRepository;
        public PaymentController() {
            _PaymentRepository = new PaymentRepository();
            _ClassRepository = new ClassRepository();
            _ClassStudentRepository = new ClassStudentRepository();
            Mapper.CreateMap<Payment, PaymentModel>();
            Mapper.CreateMap<PaymentModel, Payment>();
        }
        // GET: Home/Payment
        public ActionResult Payments()
        {
            return View();
            
        }

        [HttpPost]
        public ActionResult GetPage_Payments([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;
            searchBy = searchBy.Replace("PaymentMethodName", "PaymentMethodID");
            var data = _PaymentRepository.Get_Payments(out totalRows, pageIndex,pageSize,orderBy,searchBy);
            foreach(var item in data)
            {
                item.PaymentMethodName = StaticDataHelper.PaymentMethod.FirstOrDefault(x => x.Key == item.PaymentMethodID)?.Value;
            }    
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Payment(long? id)
        {
            ViewBag.Class = _ClassRepository.Get_AllClasses().ToList();
            if (id == null)
            {
                
                var model = new PaymentModel();
                model.PaymentDate = DateTime.Now;
                model.Title = "Thêm mới học phí sinh viên";
                model.IsEdit = false;
                return PartialView("_PaymentPopup", model);
            }
            else
            {
                var Payment = _PaymentRepository.Get_PaymentByPaymentID((long)id);
                var model = Mapper.Map<Payment, PaymentModel>(Payment);
                model.Title = "Cập nhập học phí sinh viên ";
                model.IsEdit = true;
                return PartialView("_PaymentPopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostPayment")]
        public ActionResult PostPayment(PaymentModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                if (model.IsEdit == true)
                {
                    var Payment = Mapper.Map<PaymentModel, Payment>(model); 
                    _PaymentRepository.Update(Payment);

                    return Json(new { success = true, message = "Cập nhập học phi sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Payment = Mapper.Map<PaymentModel, Payment>(model); 
                    _PaymentRepository.Insert(Payment);

                    return Json(new { success = true, message = "Thêm mới học sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeletePayment")]
        public ActionResult DeletePayment(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _PaymentRepository.Delete(id);
                var message = "Xóa bản ghi học phí sinh viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sinh viên đã được dùng ở chức năng khác" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetStudent_ByClass(long id)
        {
            try
            {
                var students = _ClassStudentRepository.Get_StudentInClass(id).ToList();  
                return Json(new { success = true, data = students }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sinh viên đã được dùng ở chức năng khác" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}