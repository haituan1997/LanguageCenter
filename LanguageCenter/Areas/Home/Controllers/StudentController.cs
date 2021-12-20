using LanguageCenter.Areas.Home.Models.StudentModel;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DataTables.Mvc;
using LanguageCenter.Code.Helper.DatatableHelper;
using LanguageCenter.Helper;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.OpenXml4Net.OPC;
using System.Configuration;
using LanguageCenter.Code.Helper.NpoiHelper;
using NPOI.SS.UserModel;
using System.Text;
using LanguageCenter.Areas.Home.Views.FilesHistoryImport;
using System.Globalization;

namespace LanguageCenter.Areas.Home.Controllers
{
    [CustomAuthorize("3")]//thì chỗ này mình điền tên controller vào
    public class StudentController : BaseController
    {
        private readonly StudentRepository _StudentRepository;
        private readonly FileHistoryImportRepository _fileHistoryImportRepository;
        public StudentController()
        {
            _StudentRepository = new StudentRepository();
            _fileHistoryImportRepository = new FileHistoryImportRepository();
            Mapper.CreateMap<Student, StudentModel>();
            Mapper.CreateMap<StudentModel, Student>();
            Mapper.CreateMap<ImportStudentModel, Student>();
            Mapper.CreateMap<FilesHistoryImport, FileHistoryImport>();
        }
        // GET: Home/Student
        public ActionResult Students()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPage_Students([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _StudentRepository.Get_Students(out totalRows, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Student(long? id)
        {
            if (id == null)
            {

                var model = new StudentModel();
                model.DateOfBirth = DateTime.Now;
                model.Title = "Thêm mới sinh viên";
                model.IsEdit = false;
                return PartialView("_StudentPopup", model);
            }
            else
            {
                var Student = _StudentRepository.Get_StudentByStudentID((long)id);
                var model = Mapper.Map<Student, StudentModel>(Student);
                model.Title = "Cập nhập sinh viên";
                model.IsEdit = true;
                return PartialView("_StudentPopup", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("PostStudent")]
        public ActionResult PostStudent(StudentModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {
                if (model.IsEdit == true)
                {
                    var Student = Mapper.Map<StudentModel, Student>(model);
                    _StudentRepository.Update(Student);

                    return Json(new { success = true, message = "Cập nhập sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var Student = Mapper.Map<StudentModel, Student>(model);
                    _StudentRepository.Insert(Student);

                    return Json(new { success = true, message = "Thêm mới sinh viên thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeleteStudent")]
        public ActionResult DeleteStudent(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _StudentRepository.Delete(id);
                var message = "Xóa sinh viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sinh viên đã được dùng ở chức năng khác" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DownloadTemplate()
        {
            try
            {
                var templateWorkbook = LoadDataToExcel();
                var ms = new MemoryStream();
                templateWorkbook.Write(ms);

                var excelTemplateFileNameDownload = $"import_hoc_sinh_{DateTime.Now.ToString("ddMMyyyy")}_{DateTime.Now.ToString("HHmmss")}.xlsx";
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
            var testValidation = NpoiDataValidation.SetValueToDropListDownCell(sheetDataImport, nameof(b), "M", "Test", 2, 202);

            #endregion

            //pass word file 
            //string passTemplate = ConfigurationManager.AppSettings.Get("TemplatePass");
            //sheetCategory.ProtectSheet(passTemplate);

            return templateWorkbook;
        }
        #endregion

        #region Import
        [HttpPost]
        public ActionResult ImportExcelFile()
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

                var students = _StudentRepository.GetAll_Students().ToArray();

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
                        var success = ImportStudents(entity, invalidStudentModels);
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
                    var fileName = $"{User.Identity.Name}_import_hoc_sinh_{DateTime.Now.ToString("ddMMyyyy")}_{DateTime.Now.ToString("HHmmss")}_error.xlsx";
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
        private bool ImportStudents(ImportStudentModel item, List<ImportStudentModel> invalidStudentModels)
        {
            if (item.CanBeImported())
            {
                try
                {
                    string[] formats = { "dd/MM/yyyy" };
                    var student = new Student()
                    {
                        FirtName = item.FirtName,
                        LastName = item.LastName,
                        DateOfBirth = DateTime.ParseExact(item.DateOfBirth, formats, new CultureInfo("en-US"), DateTimeStyles.None),
                        Email = item.Email,
                        PhoneNumber = item.PhoneNumber,
                        CurrentAddress = item.CurrentAddress,
                    };
                    _StudentRepository.Insert(student);
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
                ControllerName = nameof(Student),
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

        #region ExportData
        public ActionResult ExportData()
        {
            var excelTemplateFile = OPCPackage.Open(Server.MapPath(@"\Code\TemplateExport\template_export_hoc_sinh.xlsx"));
            var templateWorkbook = new XSSFWorkbook(excelTemplateFile);
            var sheet = templateWorkbook.GetSheet("hoc_sinh");
            var datas = _StudentRepository.GetAll_Students();
            AddObjects(
                sheet, datas.ToList(),
                _ => _.StudentID,
                _ => _.FirtName,
                _ => _.LastName,
                _ => _.DateOfBirth?.ToString("dd/MM/yyyy"),
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
    }
}