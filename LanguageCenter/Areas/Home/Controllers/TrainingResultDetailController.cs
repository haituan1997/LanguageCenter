using LanguageCenter.Areas.Home.Models.TrainingResultDetailModel;
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
    [CustomAuthorize("2,3")]
    public class TrainingResultDetailController : Controller
    {
        private readonly TrainingResultDetailRepository _TrainingResultDetailRepository;
        private readonly ClassRepository _classRepository;
        private readonly ClassStudentRepository _classStudentRepository;
        public TrainingResultDetailController() {
            _TrainingResultDetailRepository = new TrainingResultDetailRepository();
            _classRepository = new ClassRepository();
            _classStudentRepository = new ClassStudentRepository();
            Mapper.CreateMap<TrainingResultDetail, TrainingResultDetailModel>();
            Mapper.CreateMap<TrainingResultDetailModel, TrainingResultDetail>();
        }
        // GET: Home/TrainingResultDetail
        public ActionResult TrainingResultDetails()
        {
            return View();
            
        }

     
        public ActionResult GetPage_TrainingResultDetails([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, long id)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _TrainingResultDetailRepository.Get_TrainingResultDetails(out totalRows, id, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostTrainingResultDetail")]
        public ActionResult PostTrainingResultDetail(TrainingResultDetailModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                if (model.IsEdit == true)
                {
                    var TrainingResultDetail = Mapper.Map<TrainingResultDetailModel, TrainingResultDetail>(model);
                    _TrainingResultDetailRepository.Update(TrainingResultDetail);

                    return Json(new { success = true, message = "Cập nhật điểm cho học sinh thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var TrainingResultDetail = Mapper.Map<TrainingResultDetailModel, TrainingResultDetail>(model);
                    _TrainingResultDetailRepository.Insert(TrainingResultDetail);

                    return Json(new { success = true, message = "Thêm mới điểm cho học sinh thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeleteTrainingResultDetail")]
        public ActionResult DeleteTrainingResultDetail(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _TrainingResultDetailRepository.Delete(id);
                var message = "Xóa sinh viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Dữ liệu không thể xóa vui lòng xem lại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}