using AutoMapper;
using LanguageCenter.Areas.Home.Models.StudentModel;
using LanguageCenter.Areas.Home.Models.TrainingResultDetailModel;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class StudentInfomationController : BaseController
    {
        private readonly StudentRepository _studentRepository;
        private readonly StudentAccountRepository _studentAccountRepository;
        private readonly TrainingResultDetailRepository _trainingResultDetailRepository;
        public StudentInfomationController()
        {
            _studentRepository = new StudentRepository();
            _studentAccountRepository = new StudentAccountRepository();
            _trainingResultDetailRepository = new TrainingResultDetailRepository();
            Mapper.CreateMap<Student, StudentModel>();
            Mapper.CreateMap<TrainingResultDetail, TrainingResultDetailModel>();
        }
        // GET: Home/StudentInfomation
        public ActionResult StudentInfomations()
        {
            ViewBag.StudentID = StudentID;
            return View();
        }

        public ActionResult StudentInfomation(long studentID)
        {
            var student = _studentRepository.Get_StudentByStudentID(studentID);
            var studentAccount = _studentAccountRepository.Get_StudentAccountByStudentID(studentID);
            var model = Mapper.Map<StudentModel>(student);
            model.UserLogin = studentAccount?.UserLogin;
            model.PassWordLogin = studentAccount?.PassWordLogin;
            return View(model);
        }
        public ActionResult ReportForStudent(long studentID)
        {
            var trainingResultDetails = _trainingResultDetailRepository.Get_TrainingResultDetai_By_StudentID(studentID);
            ViewBag.TrainingResultDetails = Mapper.Map<IEnumerable<TrainingResultDetailModel>>(trainingResultDetails);
            return View();
        }
    }
}