using LanguageCenter.Areas.Home.Models.TrainingResultModel;
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
using LanguageCenter.Areas.Home.Models.TrainingResultDetailModel;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class TrainingResultController : Controller
    {
        private readonly TrainingResultRepository _TrainingResultRepository;
        private readonly TrainingResultDetailRepository _TrainingResultDetailRepository;
        private readonly ClassRepository _classRepository;
        private readonly ClassStudentRepository _classStudentRepository;
        public TrainingResultController() {
            _TrainingResultRepository = new TrainingResultRepository();
            _TrainingResultDetailRepository = new TrainingResultDetailRepository();
            _classRepository = new ClassRepository();
            _classStudentRepository = new ClassStudentRepository();
            Mapper.CreateMap<TrainingResult, TrainingResultModel>();
            Mapper.CreateMap<TrainingResultModel, TrainingResult>();
            Mapper.CreateMap<TrainingResultDetail, TrainingResultDetailModel>();
            Mapper.CreateMap<TrainingResultDetailModel, TrainingResultDetail>();
        }
        // GET: Home/TrainingResult
        public ActionResult TrainingResults()
        {
            return View();
            
        }

        [HttpPost]
        public ActionResult GetPage_TrainingResults([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _TrainingResultRepository.Get_TrainingResults(out totalRows, pageIndex,pageSize,orderBy,searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

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
        [HttpGet]
        public ActionResult TrainingResult(long? id)
        {
           var  classes = _classRepository.Get_AllClassesNotTrainingResult().ToList();
            if (id == null)
            {
                ViewBag.Class = classes;
                var model = new TrainingResultModel()
                {
                    ClassID=0,
                    IsCreated= false
                }; 
                model.IsEdit = false;
                var data = Mapper.Map<TrainingResultModel,TrainingResult>(model);
                model .TrainingResultID= _TrainingResultRepository.Insert(data);
                return View("_TrainingResult", model);
            }
            else
            {
                
                var TrainingResult = _TrainingResultRepository.Get_TrainingResultByTrainingResultID((long)id);
                classes.Add(_classRepository.Get_ClassByClassID(TrainingResult.ClassID));
                ViewBag.Class = classes;
                var model = Mapper.Map<TrainingResult, TrainingResultModel>(TrainingResult); 
                model.IsEdit = true;
                return View("_TrainingResult", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("TrainingResult")]
        public ActionResult TrainingResult(TrainingResultModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
               
                var TrainingResult = Mapper.Map<TrainingResultModel, TrainingResult>(model);
                TrainingResult.IsCreated = true;
                _TrainingResultRepository.Update(TrainingResult);
                return this.RedirectToAction("TrainingResults", "TrainingResult");
                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeleteTrainingResult")]
        public ActionResult DeleteTrainingResult(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _TrainingResultRepository.Delete(id);
                var message = "Xóa sinh viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sinh viên đã được dùng ở chức năng khác" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult TrainingResultDetail(long? id, long? trainingResultDetailID )
        {

            var TrainingResultDetail = _TrainingResultDetailRepository.Get_TrainingResultDetailByTrainingResultDetailID((long)id);
            var TrainingResult = _TrainingResultRepository.Get_TrainingResultByTrainingResultID(TrainingResultDetail.TrainingResultID);
            ViewBag.ClassStudent = _classStudentRepository.Get_StudentInClassNotInTrainingResult(TrainingResultDetail.TrainingResultID, TrainingResult.ClassID, TrainingResultDetail.TrainingResultDetailID).ToList();

            var model = Mapper.Map<TrainingResultDetail, TrainingResultDetailModel>(TrainingResultDetail);
            model.IsEdit = true;
            return PartialView("_trainingResultPopup", model);
        }
        [HttpGet]
        public ActionResult InsertTrainingResultDetail(long id,long classID)
        {
            var model = new TrainingResultDetailModel();
            model.IsEdit = false;
            model.TrainingResultID = id; 
            ViewBag.ClassStudent = _classStudentRepository.Get_StudentInClassNotInTrainingResult(id,classID, 0).ToList();

            return PartialView("_trainingResultPopup", model);
            
        }
       
    }
}