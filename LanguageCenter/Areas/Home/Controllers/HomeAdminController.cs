using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{
    [Authorize]
    public class HomeAdminController : Controller
    {
         
        // GET: Home/Home
        public HomeAdminController()
        {

        }
        public ActionResult Index()
        {
            return View();

        }
        
    }
}