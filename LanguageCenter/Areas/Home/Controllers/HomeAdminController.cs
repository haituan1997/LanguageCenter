using LanguageCenter.Code.Helper.DatatableHelper;
using DataTables.Mvc;
using LanguageCenter.Models;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using LanguageCenter.Helper;

namespace LanguageCenter.Areas.Home.Controllers
{
    [CustomAuthorize("2,3")]
    public class HomeAdminController : Controller
    {
        private readonly UserRepository _UserRepository;
        // GET: Home/Home
        public HomeAdminController()  
        {
            _UserRepository = new UserRepository();
        }
        public ActionResult Index()
        {
            
            return View();

        }
      

        public JsonResult GetEvents()
        {
            var model = new List<Event>();
            model.Add(new Event()
            {
                EventID = 1,
                Subject = "test",
                Description = "test1",
                Start = DateTime.Now,
                ThemeColor = "red",
                IsFullDay = true
            });
            return new JsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public partial class Event
        {
            public int EventID { get; set; }
            public string Subject { get; set; }
            public string Description { get; set; }
            public System.DateTime Start { get; set; }
            public Nullable<System.DateTime> End { get; set; }
            public string ThemeColor { get; set; }
            public bool IsFullDay { get; set; }
        }
    }
}