using AutoMapper;
using DataTables.Mvc;
using LanguageCenter.Areas.Home.Models.FilterPrice;
using LanguageCenter.Code.Helper.DatatableHelper;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class FilterPriceController : Controller
    {
        private static CourseRepository _courseRepository;
        private static PaymentRepository _paymentRepository;
        public FilterPriceController()
        {
            _courseRepository = new CourseRepository();
            _paymentRepository = new PaymentRepository();
            Mapper.CreateMap<Payment,FilterPriceModel>();
        }
        // GET: Home/FilterPrice
        public ActionResult FilterPrices()
        {
            ViewBag.Courses = _courseRepository.Get_AllCourses();
            return View();
        }

        [HttpPost]
        public ActionResult Paged_FilterPrices([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var param = JsonConvert.DeserializeObject<FilterPrice>(requestModel.Search.Value);
            var requestForm = Request.Form;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var filter = DatatableHelper.GetFillter<FilterPrice>(requestModel, requestForm);
            filter.Paged = requestParams.PageIndex;
            filter.PagedSize = requestParams.PageSize;
            filter.OrderByColumn = requestParams.OrderBy;
            filter.ClassID = param?.ClassID;
            filter.CourseID = param?.CourseID;
            var datas = _paymentRepository.GetPaged_FilterPrice(filter);
            var model = Mapper.Map<IEnumerable<FilterPriceModel>>(datas.Item1.ToList());
            var totalColumn = 0;
            totalColumn = Convert.ToInt32(datas.Item2[0]);

            return Json(new
            {
                draw = requestModel.Draw,
                recordsFiltered = totalColumn,
                recordsTotal = totalColumn,
                data = model,
            }, JsonRequestBehavior.AllowGet);
        }

    }
}