using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class FileHistoryImportController : BaseController
    {
        private readonly FileHistoryImportRepository _fileHistoryImportRepository;
        public FileHistoryImportController()
        {
            _fileHistoryImportRepository = new FileHistoryImportRepository();
        }
        // GET: Home/FileHistoryImport
        public ActionResult GetByContronllerAndUserId(string controllerName)
        {
            var files = _fileHistoryImportRepository.GetByContronllerAndUserId(controllerName, UserID, (byte)TypeUser);
            return Json(new { status = true, data = files }, JsonRequestBehavior.AllowGet);
        }
    }
}