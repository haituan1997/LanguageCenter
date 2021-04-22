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
  
    public class HomeController : Controller
    {

        
        // GET: Home/Home
        private readonly ClassRepository _ClassRepository;
        public HomeController()
        {
            _ClassRepository = new ClassRepository();
             

        }
        public ActionResult Index()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
             
            return View();

        }
        
    }
}