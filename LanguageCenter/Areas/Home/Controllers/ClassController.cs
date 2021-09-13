using AutoMapper;
using DataTables.Mvc;
using LanguageCenter.Areas.Home.Models.Class;
using LanguageCenter.Areas.Home.Models.ClassStudent;
using LanguageCenter.Areas.Home.Models.StudentModel;
using LanguageCenter.Areas.Home.Models.TeacherModel;
using LanguageCenter.Areas.Home.Views.FilesHistoryImport;
using LanguageCenter.Code.Helper.DatatableHelper;
using LanguageCenter.Code.Helper.NpoiHelper;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.Helper;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using NPOI.OpenXml4Net.OPC;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static Code.Enumerator.Enumerator;

namespace LanguageCenter.Areas.Home.Controllers
{
    public class ClassController : BaseController
    {
        private readonly TeacherRepository _teacherRepository;
        private readonly ClassRepository _classRepository;
        private readonly CourseRepository _courseRepository;
        private readonly ClassStudentRepository _classStudentRepository;
        private readonly RegistrationClassRepository _registrationClassRepository;
        private readonly StudentRepository _studentRepository;
        private readonly FileHistoryImportRepository _fileHistoryImportRepository;

        public ClassController()
        {
            _teacherRepository = new TeacherRepository();
            _classRepository = new ClassRepository();
            _courseRepository = new CourseRepository();
            _classStudentRepository = new ClassStudentRepository();
            _registrationClassRepository = new RegistrationClassRepository();
            _studentRepository = new StudentRepository();
            _fileHistoryImportRepository = new FileHistoryImportRepository();
            Mapper.CreateMap<Teacher, TeacherModel>();
            Mapper.CreateMap<Class, NewClassModel>();
            Mapper.CreateMap<Class, ClassModel>();
            Mapper.CreateMap<NewClassModel, Class>();
            Mapper.CreateMap<ClassStudent, ClassStudentModel>();
            Mapper.CreateMap<ClassStudentModel, ClassStudent>();
            Mapper.CreateMap<FilesHistoryImport, FileHistoryImport>();
            Mapper.CreateMap<ImportStudentModel, Student>();
        }

        // GET: Home/Class
        public ActionResult Classes()
        {
            if (TeacherID != -1)
                ViewBag.Function = new Tuple<int, int>((int)TypeOfPermission.Type0, (int)TypeOfPermission.Type1);
            return View();
        }
        [HttpPost]
        public ActionResult GetPage_Classes([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;
            if (searchBy.Contains("FullName"))
            {
                searchBy = searchBy.Replace("FullName", "LastName");
            }
            if (orderBy.Contains("FullName"))
            {
                orderBy = orderBy.Replace("FullName", "LastName");
            }
            if (searchBy.Contains("CourseName"))
            {
                searchBy = searchBy.Replace("CourseName", "Name");
            }
            if (orderBy.Contains("CourseName"))
            {
                orderBy = orderBy.Replace("CourseName", "Name");
            }

            if (TeacherID != -1)
                searchBy += " AND cl.TeacherID =" + TeacherID;

            var data = _classRepository.Get_Classes(out totalRows, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult Class(long? id)
        {
            ViewBag.Courses = _courseRepository.Get_AllCourses().ToList();
            ViewBag.Teachers = _teacherRepository.Get_AllTeacheres().ToList();
            if (id == null)
            {
                var model = new Class
                {
                    Title = "Thêm mới lớp học",
                    IsCreated = false,
                    //TeacherID = 0,
                    //CourseID = 0,
                };
                var newModel = new NewClassModel
                {
                    ClassID = _classRepository.Insert(model),
                    Title = model.Title,
                    IsCreated = model.IsCreated,
                };
                ViewBag.ClassID = newModel.ClassID;
                return View("Class", newModel);
            }
            else
            {
                var obj = _classRepository.Get_ClassByClassID((long)id);
                var model = Mapper.Map<Class, NewClassModel>(obj);
                model.Title = "Cập nhập lớp học";
                model.IsEdit = true;
                ViewBag.ClassID = model.ClassID;
                return View("Class", model);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("Class")]
        public ActionResult Class(NewClassModel model)
        {
            ViewBag.Courses = _courseRepository.Get_AllCourses().ToList();
            ViewBag.Teachers = _teacherRepository.Get_AllTeacheres().ToList();
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Đã có lỗi xảy ra";
                return View("Class", model);
            }
            try
            {
                if (model.IsCreated)
                {
                    var objClass = Mapper.Map<NewClassModel, Class>(model);
                    _classRepository.Update(objClass);
                    TempData["Success"] = "Cập nhập lớp thành công!";
                }
                else
                {
                    var objClass = Mapper.Map<NewClassModel, Class>(model);
                    objClass.IsCreated = true;
                    _classRepository.Update(objClass);
                    TempData["Success"] = "Thêm mới lớp thành công!";
                }

                return RedirectToAction("Classes", "Class", new { id = "" });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View("Class", model);
            }
        }
        [ActionName("DeleteClass")]
        public ActionResult DeleteClass(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _classRepository.Delete(id);
                var message = "Xóa lớp học thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetPage_StudenByClassID([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, long? classID)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;
            if (searchBy.Contains("FullName"))
            {
                searchBy = searchBy.Replace("FullName", "LastName");
            }
            if (orderBy.Contains("FullName"))
            {
                orderBy = orderBy.Replace("FullName", "LastName");
            }

            var data = _classStudentRepository.Get_ClassStudentByClassID(classID, out totalRows, pageIndex, pageSize, orderBy, searchBy);

            return Json(new
            {
                draw = requestModel.Draw,
                recordsTotal = totalRows,
                recordsFiltered = totalRows,
                data = data.ToArray()
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ClassStudent(long id)
        {
            ViewBag.StudentNotIntClass = _classStudentRepository.Get_StudentNotInClass(id);
            var model = new ClassStudentModel();
            model.Title = "Chọn học sinh";
            model.ClassID = id;
            return PartialView("_ClassStudent", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("ClassStudent")]
        public ActionResult PostClassStudent(ClassStudent model)
        {
            ViewBag.StudentNotIntClass = _classStudentRepository.Get_StudentNotInClass(model.ClassID);

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra!" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                _classStudentRepository.Insert(model);
                return Json(new { success = true, message = "Thêm mới học sinh vào lớp thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [ActionName("DeleteClassStudent")]
        public ActionResult DeleteClassStudent(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _classStudentRepository.Delete(id);
                var message = "Xóa học sinh khỏi lớp thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RegistrationClass()
        {
            return PartialView("_RegistrationClass");
        }
        public ActionResult GetPage_RegistrationClassByClassID([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, long? classID)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;
            if (searchBy.Contains("FullName"))
            {
                searchBy = searchBy.Replace("FullName", "LastName");
            }
            if (orderBy.Contains("FullName"))
            {
                orderBy = orderBy.Replace("FullName", "LastName");
            }
            var datas = _registrationClassRepository.GetPage_RegistrationClassByClassID(classID, pageIndex, pageSize, orderBy, searchBy);

            var data = datas.Item1;
            totalRows = Convert.ToInt32(datas.Item2[0]);

            return Json(new
            {
                draw = requestModel.Draw,
                recordsTotal = totalRows,
                recordsFiltered = totalRows,
                data = data.ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult StudentRegistrationClass(long studentId, long classId)
        {
            try
            {
                var model = new ClassStudent
                {
                    StudentID = studentId,
                    ClassID = classId
                };
                _classStudentRepository.Insert(model);
                return Json(new { success = true, message = "Đã đăng ký thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        #region ExportData
        public ActionResult ExportData(long classId)
        {
            var excelTemplateFile = OPCPackage.Open(Server.MapPath(@"\Code\TemplateExport\template_export_hoc_sinh.xlsx"));
            var templateWorkbook = new XSSFWorkbook(excelTemplateFile);
            var sheet = templateWorkbook.GetSheet("hoc_sinh");
            var datas = _classStudentRepository.GetData_For_Export(classId);
            AddObjects(
                sheet, datas.ToList(),
                _ => _.StudentID,
                _ => _.FirtName,
                _ => _.LastName,
                _ => _.DateOfBirth.ToString("dd/MM/yyyy"),
                _ => _.PhoneNumber,
                _ => _.CurrentAddress,
                _ => _.Email
            );
            var stream = new MemoryStream();
            templateWorkbook.Write(stream);

            var excelTemplateFileNameDownload = $"danh_sach_hoc_sinh_{DateTime.Now:ddMMyyyy}_{DateTime.Now:HHmmss}.xlsx";
            return File(stream.ToArray(), "application/vnd.ms-excel", excelTemplateFileNameDownload);
        }
        protected void AddObjects<T>(ISheet sheet, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            var cellStyle = sheet.Workbook.CreateCellStyle();
            var font = sheet.Workbook.CreateFont();
            font.FontHeightInPoints = 12;
            font.FontName = "Times New Roman";
            font.IsBold = false;
            cellStyle.SetFont(font);
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.Alignment = HorizontalAlignment.Left;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.WrapText = true;

            for (var i = 0; i < items.Count; i++)
            {
                var row = sheet.CreateRow(i + 2);

                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    var cell = row.CreateCell(j);
                    var value = propertySelectors[j](items[i]);
                    if (value != null)
                    {
                        cell.SetCellValue(value.ToString());
                    }
                    cell.CellStyle = cellStyle;
                }
            }
        }
        #endregion

        [HttpGet]
        public ActionResult DownloadTemplate()
        {
            try
            {
                var templateWorkbook = LoadDataToExcel();
                var ms = new MemoryStream();
                templateWorkbook.Write(ms);

                var excelTemplateFileNameDownload = $"import_hoc_sinh_vao_lop_{DateTime.Now.ToString("ddMMyyyy")}_{DateTime.Now.ToString("HHmmss")}.xlsx";
                return File(ms.ToArray(), "application/vnd.ms-excel", excelTemplateFileNameDownload);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region Tải file mẫu
        private XSSFWorkbook LoadDataToExcel()
        {
            var excelTemplateFile = OPCPackage.Open(Server.MapPath(@"\Code\TemplateImport\template_import_hoc_sinh.xlsx"));
            var templateWorkbook = new XSSFWorkbook(excelTemplateFile);
            var sheetCategory = templateWorkbook.GetSheet("danh_muc");

            #region add value to cell
            sheetCategory.ForceFormulaRecalculation = true;
            var a = new List<string>();
            a.Add("Test");
            a.Add("Test1");
            var b = a.ToArray();
            b.CreateDropDownUsingCellReference(templateWorkbook, sheetCategory, nameof(b), "B-4");
            #endregion

            #region set value to drop list down
            var sheetDataImport = templateWorkbook.GetSheet("hoc_sinh");
            var testValidation = new NpoiDataValidation((XSSFSheet)sheetDataImport, nameof(b), 202, 2, "M", "M");
            testValidation.InitValidationBox("Test", "Test", "Test", "Test");
            testValidation.AddValidation();
            #endregion

            //pass word file 
            //string passTemplate = ConfigurationManager.AppSettings.Get("TemplatePass");
            //sheetCategory.ProtectSheet(passTemplate);

            return templateWorkbook;
        }
        #endregion

        #region Import
        [HttpPost]
        public ActionResult ImportExcelFile(long classId)
        {
            try
            {
                HttpPostedFileBase files = Request.Files[0];
                XSSFWorkbook workbook = new XSSFWorkbook(files.InputStream);
                if (workbook.NumberOfSheets < 2)
                {
                    return Json(new
                    {
                        status = false,
                        errorMessage = "Import thất bại do tệp tin không phù hợp!",
                        rowTotal = 0,

                    }, JsonRequestBehavior.AllowGet);
                }

                var sheet = workbook.GetSheetAt(0);
                var sheet2 = workbook.GetSheetAt(1);

                if (sheet.GetRow(0).GetCell(0) == null)
                    throw new Exception("Import thất bại do tệp tin không phù hợp!");

                var isValidTemplate = IsValidTemplate(sheet.GetRow(1));
                if (!isValidTemplate)
                {
                    return Json(new
                    {
                        status = false,
                        errorMessage = "Import thất bại do tệp tin không phù hợp!",
                        rowTotal = 0,
                    }, JsonRequestBehavior.AllowGet);
                }

                //var validKey = sheet2.GetRow(0).GetCell(0).StringCellValue;
                //var validTemplateFile = ConfigurationManager.AppSettings.Get("ValidTemplateFile");
                //if (validKey != validTemplateFile)
                //    throw new Exception("Import thất bại do tệp tin không phù hợp!");

                var students = _studentRepository.GetAll_Students().ToArray();

                int totalRow = sheet.LastRowNum;

                var invalidStudentModels = new List<ImportStudentModel>();
                int numberOfSuccessRow = 0;
                int actualRow = 0;

                for (int rowIndex = 2; rowIndex <= totalRow; rowIndex++)
                {
                    var currentRow = sheet.GetRow(rowIndex);
                    if (currentRow == null)
                        continue;

                    if (currentRow != null && currentRow.Cells.All(x => x.CellType == CellType.Blank))
                        continue;

                    actualRow++;
                    var entity = ProcessExcelRow(sheet, rowIndex, students);

                    if (entity != null)
                    {
                        var success = ImportStudents(entity, invalidStudentModels, classId);
                        if (success) numberOfSuccessRow++;
                    }
                }

                if (actualRow == 0)
                {
                    return Json(new
                    {
                        status = false,
                        errorMessage = "Tệp tin có dữ liệu trống, bạn vui lòng chọn lại tệp tin!",
                        rowTotal = 0,

                    }, JsonRequestBehavior.AllowGet);
                }

                var message = $"Tải lên hoàn tất { numberOfSuccessRow }/{ actualRow } bản ghi";
                if (invalidStudentModels.Any())
                {
                    var fileName = $"{User.Identity.Name}_import_hoc_sinh_vao_lop_{DateTime.Now.ToString("ddMMyyyy")}_{DateTime.Now.ToString("HHmmss")}_error.xlsx";
                    SaveInvalidRowToDisk(invalidStudentModels, fileName);

                    return Json(new
                    {
                        status = false,
                        message,
                        nameFile = fileName
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    status = true,
                    message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    errorMessage = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        private ImportStudentModel ProcessExcelRow(ISheet worksheet, int row, Student[] students)
        {
            var upto255Characters = 255;
            var exceptionMessage = new StringBuilder();
            var model = new ImportStudentModel();

            try
            {
                model.FirtName = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 0, "Họ", true, upto255Characters, exceptionMessage);
                model.LastName = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 1, "Tên", true, upto255Characters, exceptionMessage);
                if (students.Where(x => NpoiImportHelper.CheckCompareText(x.FirtName, model.FirtName) && NpoiImportHelper.CheckCompareText(x.LastName, model.LastName)).Any())
                {
                    exceptionMessage.Append("Họ và tên đã tồn tại trong hệ thống; ");
                }
                model.DateOfBirth = NpoiImportHelper.GetDateFormatCellValueFromRow(worksheet, row, 2, "Ngày sinh", true, exceptionMessage);
                model.PhoneNumber = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 3, "Số điện thoại", true, upto255Characters, exceptionMessage);
                if (students.Where(x => NpoiImportHelper.CheckCompareText(x.PhoneNumber, model.PhoneNumber)).Any())
                {
                    exceptionMessage.Append("Số điện thoại đã tồn tại trong hệ thống; ");
                }
                model.CurrentAddress = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 4, "Địa chỉ", true, upto255Characters, exceptionMessage);
                model.Email = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 5, "Email", false, upto255Characters, exceptionMessage);

                model.Exception = exceptionMessage.ToString();
                return model;
            }
            catch (Exception exception)
            {
                model.Exception = exception.Message;
                return model;
            }
        }
        private bool ImportStudents(ImportStudentModel item, List<ImportStudentModel> invalidStudentModels, long classId)
        {
            if (item.CanBeImported())
            {
                try
                {
                    var student = Mapper.Map<Student>(item);
                    var studentId = Convert.ToInt64(_studentRepository.Insert(student));
                    var model = new ClassStudent
                    {
                        StudentID = studentId,
                        ClassID = classId
                    };
                    _classStudentRepository.Insert(model);
                    return true;
                }
                catch (Exception exception)
                {
                    item.Exception = exception.Message;
                    invalidStudentModels.Add(item);
                    return false;
                }
            }
            else
            {
                invalidStudentModels.Add(item);
                return false;
            }
        }
        private bool IsValidTemplate(IRow row)
        {
            try
            {
                var validcellNameArray = new List<string>
                {
                    "Họ",
                    "Tên",
                    "Ngày sinh",
                    "Số điện thoại",
                    "Địa chỉ",
                    "Email",
                };

                var needToCheckArray = new List<string>();
                for (int i = 0; i <= 5; i++)
                {
                    var cell = row.Cells[i];
                    cell.SetCellType(CellType.String);
                    var cellValue = cell.StringCellValue;

                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        needToCheckArray.Add(cellValue);
                    }
                }

                if (!validcellNameArray.SequenceEqual(needToCheckArray))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region save file import
        private void SaveInvalidRowToDisk(List<ImportStudentModel> invalidStudentModels, string fileName)
        {
            const string importHistoryFolder = "~/FileHistoryImport";
            Directory.CreateDirectory(Server.MapPath(importHistoryFolder));
            string path = Path.Combine(Server.MapPath(importHistoryFolder), fileName);

            var filesHistoryImport = new FilesHistoryImport()
            {
                ControllerName = nameof(Class),
                FileName = fileName,
                TypeUser = (byte)TypeUser,
                UserID = UserID != -1 ? UserID : TeacherID,
            };
            var model = Mapper.Map<FileHistoryImport>(filesHistoryImport);
            _fileHistoryImportRepository.Insert(model);

            var workbook = LoadDataToExcel();
            var sheet = workbook.GetSheetAt(0);

            var errorCell = sheet.GetRow(1).CreateCell(6);
            var cellStyle = sheet.Workbook.CreateCellStyle();
            var font = sheet.Workbook.CreateFont();
            font.FontHeightInPoints = 12;
            font.FontName = "Times New Roman";
            font.IsBold = true;
            cellStyle.SetFont(font);
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            errorCell.CellStyle = cellStyle;
            errorCell.SetCellValue("Lỗi");

            // Add data to sheet
            AddSheetContent(
                sheet,
                2,
                invalidStudentModels,
                _ => _.FirtName,
                _ => _.LastName,
                _ => _.DateOfBirth,
                _ => _.PhoneNumber,
                _ => _.CurrentAddress,
                _ => _.Email,
                _ => _.Exception
            );

            sheet.SetColumnWidth(19, 20000);

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }
        protected void AddSheetContent(ISheet sheet, int startRowIndex, List<ImportStudentModel> items, params Func<ImportStudentModel, object>[] propertySelectors)
        {
            var cellStyle = sheet.Workbook.CreateCellStyle();
            var font = sheet.Workbook.CreateFont();
            font.FontHeightInPoints = 12;
            font.FontName = "Times New Roman";
            cellStyle.SetFont(font);
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            for (var i = 0; i < items.Count; i++)
            {
                var row = sheet.CreateRow(startRowIndex++);

                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    if (j == propertySelectors.Length - 1)
                    {
                        cellStyle.WrapText = true;
                    }

                    var cell = row.CreateCell(j);
                    cell.CellStyle = cellStyle;

                    var value = propertySelectors[j](items[i]);
                    if (value != null)
                    {
                        cell.SetCellValue(value.ToString());
                    }
                }
            }
        }
        #endregion end save file import

        #endregion end import
    }
}