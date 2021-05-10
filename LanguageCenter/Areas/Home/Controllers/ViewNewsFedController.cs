using AutoMapper;
using LanguageCenter.Areas.Home.Models.NewsFeedModel;
using LanguageCenter.Areas.Home.Models.Class;
using LanguageCenter.Areas.Home.Models.Course;
using LanguageCenter.Areas.Home.Models.TeacherModel;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.ViewNewsFed.Controllers
{

    public class ViewNewsFedController : Controller
    {

        // GET: ViewNewsFed/ViewNewsFed
        private readonly NewsFeedRepository _NewsFeedRepository;
        public ViewNewsFedController()
        {
            _NewsFeedRepository = new NewsFeedRepository();
            Mapper.CreateMap<NewsFeed, NewsFeedModel>();
        }
        
        public ActionResult Index(string slug)
        {
            int total = 0;
            var newsfed = _NewsFeedRepository.Get_NewsFeeds(out total, 1, 10, null, null).ToList();
            foreach (var item in newsfed)
            {
                item.Thumb = Getthum(item.Description);
            }

            newsfed.Remove(newsfed.FirstOrDefault(x=>x.Code==slug));

            ViewBag.newFeds = newsfed;
            ViewBag.Total = total;

            var data = _NewsFeedRepository.Get_NewsFeedByCode("ong-mat-troi");
            var model = Mapper.Map<NewsFeed, NewsFeedModel>(data);
            model.Thumb = Getthum(model.Description);
            return View(model);
        }
        public string Getthum(string value)
        {
            if (value.IndexOf("<img") >= 0)
            {
                value = value.Substring(value.IndexOf("<img"), value.Length - value.IndexOf("<img"));
                value = value.Substring(value.IndexOf("src=") + 5, value.Length - value.IndexOf("src=") - 5);
                value = value.Substring(0, value.IndexOf('"'));
            }
            return value;
        }
        public ActionResult GetViewNewsFed(int page)
        {
            int total = 0;
            var newsfed = _NewsFeedRepository.Get_NewsFeeds(out total, page, 10, null, null).ToList();
            foreach (var item in newsfed)
            {
                item.Thumb = Getthum(item.Description);
                item.CreatedateView = item.Createdate.ToString("dd/MM/yyyy");
            }

            return Json(new { success = true, data = newsfed }, JsonRequestBehavior.AllowGet);

        }

    }
}