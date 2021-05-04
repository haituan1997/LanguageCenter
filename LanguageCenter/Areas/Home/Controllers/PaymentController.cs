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

namespace LanguageCenter.Areas.Home.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentRepository _PaymentRepository;
        public PaymentController() {
            _PaymentRepository = new PaymentRepository();
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

            var data = _PaymentRepository.Get_Payments(out totalRows, pageIndex,pageSize,orderBy,searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Payment(long? id)
        {
            ViewBag.Students = _StudentRepository.GetAll_Students().ToList();
            if (id == null)
            {
                
                var model = new PaymentModel();
                model.PaymentDate = DateTime.Now;
                model.Title = "Thêm mới sinh viên";
                model.IsEdit = false;
                return PartialView("_PaymentPopup", model);
            }
            else
            {
                var Payment = _PaymentRepository.Get_PaymentByPaymentID((long)id);
                var model = Mapper.Map<Payment, PaymentModel>(Payment);
                model.Title = "Cập nhập sinh viên";
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

                    return Json(new { success = true, message = "Cập nhập sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Payment = Mapper.Map<PaymentModel, Payment>(model); 
                    _PaymentRepository.Insert(Payment);

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
        [ActionName("DeletePayment")]
        public ActionResult DeletePayment(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _PaymentRepository.Delete(id);
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