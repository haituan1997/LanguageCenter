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

namespace LanguageCenter.Areas.Home.Controllers
{
    [Authorize]
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
        public JsonResult getpage([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;

            var data = new List<UserModel1>();
            data.Add(new UserModel1()
            {
                test=1,
                UserID=1,
                FullName="lê hoa 1"
            });
            data.Add(new UserModel1()
            {
                UserID = 2,
                FullName = "lê hoa 2"
            });
            data.Add(new UserModel1()
            {
                UserID = 3,
                FullName = "lê hoa 3"
            });
            data.Add(new UserModel1()
            {
                UserID = 4,
                FullName = "lê hoa 4"
            });
            data.Add(new UserModel1()
            {
                UserID = 5,
                FullName = "lê hoa 5"
            });
            data.Add(new UserModel1()
            {
                UserID = 6,
                FullName = "lê hoa 6"
            });
            data.Add(new UserModel1()
            {
                UserID = 7,
                FullName = "lê hoa 7"
            });
            data.Add(new UserModel1()
            {
                UserID = 8,
                FullName = "lê hoa 8"
            });
            data.Add(new UserModel1()
            {
                UserID = 9,
                FullName = "lê hoa 9"
            });
            data.Add(new UserModel1()
            {
                UserID = 10,
                FullName = "lê hoa 10"
            });
            
            return Json(new { draw = requestModel.Draw, recordsTotal =15, recordsFiltered=15,  data = data.ToArray() }, JsonRequestBehavior.AllowGet);
                   

        }
       public class UserModel1
        {
            public int UserID { get; set; }
            public string FullName { get; set; }
            public int test { get; set; }
        }

    }
}