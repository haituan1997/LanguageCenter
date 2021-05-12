using LanguageCenter.Areas.Home.Models.TeacherModel;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.IO;
using System.Configuration;
using LanguageCenter.Helper;
using DataTables.Mvc;
using LanguageCenter.Code.Helper.DatatableHelper;

namespace LanguageCenter.Areas.Home.Controllers
{
    [CustomAuthorize("2,3")]
    public class TeacherController : Controller
    {
        private readonly TeacherRepository _teacherRepository;
        public TeacherController() {
            _teacherRepository = new TeacherRepository();
            Mapper.CreateMap<Teacher, TeacherModel>();
            Mapper.CreateMap<TeacherModel,Teacher >();
        }
        // GET: Home/Teacher
        public ActionResult Teacheres()
        {
            return View();
            
        }
        [HttpPost]
        public ActionResult GetPage_Teacheres([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;
            var data = _teacherRepository.Get_PagedTeacheres(out totalRows, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Teacher(long? id)
        {
            var teacher = new Teacher();
            if (id == null)
            {
                var model = Mapper.Map <Teacher, TeacherModel>(teacher);
                model.Title = "Thêm mới giáo viên";
                model.IsEdit = false;
                return PartialView("_TeacherPopup", model);
            }
            else
            {
                teacher = _teacherRepository.Get_AllTeacheres().FirstOrDefault(x => x.TeacherID == (long)id);
                var model = Mapper.Map<Teacher, TeacherModel>(teacher);
                model.Title = "Cập nhập giáo viên";
                model.IsEdit = true;
                return PartialView("_TeacherPopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostTeacher")]
        public ActionResult PostTeacher(TeacherModel model)
        {
            if (!ModelState.IsValid)
                 throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                var teacher = Mapper.Map<TeacherModel, Teacher>(model);
                
                _teacherRepository.InsertOrUpdate(teacher);
                var message = model.IsEdit == true ? "Cập nhập giáo viên thành công!" : "Thêm mới giáo viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            var fileName = "";
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    string startupPath = System.IO.Directory.GetCurrentDirectory();
                    string startupPath1 = Environment.CurrentDirectory;
                    string[] files;
                    var requiredPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)));
                    requiredPath += "\\LanguageCenter\\images\\Teachers";
                    string saveUrl = requiredPath;
                    saveUrl = saveUrl.Remove(0, 6);
                    var check = true;
                    var avatarUrl = "~/images/Teachers/" + fileName;
                    try
                    {
                        files = Directory.GetFiles(saveUrl, "*.*", SearchOption.AllDirectories);
                        if (files.Length > 0)
                        {
                            foreach (string item in files)
                            {
                                if (item.Contains(fileName))
                                {
                                    check = false;
                                    break;
                                } 
                            }
                        }
                        if (check)
                        {
                            file.SaveAs(Server.MapPath(avatarUrl));
                        }
                    }
                    catch
                    {
                        var url = ConfigurationManager.AppSettings.Get("UrlImages");
                        files = Directory.GetFiles(url, "*.*", SearchOption.AllDirectories);
                        if (files.Length > 0)
                        {
                            foreach (string item in files)
                            {
                                if (item.Contains(fileName))
                                {
                                    check = true;
                                    break;
                                }
                            }
                        }
                        if (check)
                        {
                            file.SaveAs(Server.MapPath(avatarUrl));
                        }
                    }
                }
            }
            return Json(new { fileName = fileName });
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeleteTeacher")]
        public ActionResult DeleteTeacher(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!"}, JsonRequestBehavior.AllowGet);
            try
            {
                _teacherRepository.Delete(id);
                var message = "Xóa giáo viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}