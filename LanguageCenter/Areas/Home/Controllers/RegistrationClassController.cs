using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class RegistrationClassController : BaseController
    {
        private readonly RegistrationClassRepository _registrationClassRepository;
        public RegistrationClassController()
        {
            _registrationClassRepository = new RegistrationClassRepository();
        }
        // GET: Home/RegistrationClass
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterForClasses(long classId)
        {
            try
            {
                var model = new RegistrationClass
                {
                    StudentID = StudentID,
                    ClassID = classId,
                };
                _registrationClassRepository.Insert(model);
                return Json(new { success = true, message = "Đăng ký học thành công", classID = classId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CancelRegistrater(long classId)
        {
            try
            {
                var model = new RegistrationClass
                {
                    StudentID = StudentID,
                    ClassID = classId,
                };
                _registrationClassRepository.Delete(model);
                return Json(new { success = true, message = "Hủy đăng ký học thành công",classID = classId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}