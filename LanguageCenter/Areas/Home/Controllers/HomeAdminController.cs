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
      

    }
}