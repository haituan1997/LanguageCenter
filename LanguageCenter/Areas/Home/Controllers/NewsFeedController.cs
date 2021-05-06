﻿using LanguageCenter.Areas.Home.Models.NewsFeedModel;
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
    public class NewsFeedController : Controller
    {
        private readonly NewsFeedRepository _NewsFeedRepository;
        public NewsFeedController() {
            _NewsFeedRepository = new NewsFeedRepository();
            Mapper.CreateMap<NewsFeed, NewsFeedModel>();
            Mapper.CreateMap<NewsFeedModel, NewsFeed>();
        }
        // GET: Home/NewsFeed
        public ActionResult NewsFeeds()
        {
            return View();
            
        }

        [HttpPost]
        public ActionResult GetPage_NewsFeeds([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _NewsFeedRepository.Get_NewsFeeds(out totalRows, pageIndex,pageSize,orderBy,searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult NewsFeed(long? id)
        {
            if (id == null)
            {
                
                var model = new NewsFeedModel();
                model.Title = "Thêm mới sinh viên";
                model.IsEdit = false;
                return View("_NewsFeedPopup", model);
            }
            else
            {
                var NewsFeed = _NewsFeedRepository.Get_NewsFeedByNewFeedID((long)id);
                var model = Mapper.Map<NewsFeed, NewsFeedModel>(NewsFeed);
                model.Title = "Cập nhập sinh viên";
                model.IsEdit = true;
                return View("_NewsFeedPopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostNewsFeed")]
        public ActionResult PostNewsFeed(NewsFeedModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                if (model.IsEdit == true)
                {
                    var NewsFeed = Mapper.Map<NewsFeedModel, NewsFeed>(model);
                    _NewsFeedRepository.Update(NewsFeed);

                    return Json(new { success = true, message = "Cập nhập sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var NewsFeed = Mapper.Map<NewsFeedModel, NewsFeed>(model);
                    _NewsFeedRepository.Insert(NewsFeed);

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
        [ActionName("DeleteNewsFeed")]
        public ActionResult DeleteNewsFeed(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _NewsFeedRepository.Delete(id);
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