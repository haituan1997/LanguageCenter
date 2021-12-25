using LanguageCenter.Areas.Home.Models.TrainingResultModel;
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
using LanguageCenter.Areas.Home.Models.TrainingResultDetailModel;
using LanguageCenter.Helper;
using NPOI.XSSF.UserModel;
using NPOI.OpenXml4Net.OPC;
using LanguageCenter.Code.Helper.NpoiHelper;
using System.IO;
using NPOI.SS.UserModel;
using System.Text;
using LanguageCenter.Areas.Home.Views.FilesHistoryImport;

namespace LanguageCenter.Areas.Home.Controllers
{
    //[CustomAuthorize("2,3")]
    public class TrainingResultController : BaseController
    {
        private readonly TrainingResultRepository _TrainingResultRepository;
        private readonly TrainingResultDetailRepository _TrainingResultDetailRepository;
        private readonly ClassRepository _classRepository;
        private readonly ClassStudentRepository _classStudentRepository;
        private readonly FileHistoryImportRepository _fileHistoryImportRepository;
        public TrainingResultController()
        {
            _TrainingResultRepository = new TrainingResultRepository();
            _TrainingResultDetailRepository = new TrainingResultDetailRepository();
            _classRepository = new ClassRepository();
            _classStudentRepository = new ClassStudentRepository();
            _fileHistoryImportRepository = new FileHistoryImportRepository();
            Mapper.CreateMap<TrainingResult, TrainingResultModel>();
            Mapper.CreateMap<TrainingResultModel, TrainingResult>();
            Mapper.CreateMap<TrainingResultDetail, TrainingResultDetailModel>();
            Mapper.CreateMap<TrainingResultDetailModel, TrainingResultDetail>();
            Mapper.CreateMap<TrainingResultDetail, ImportTrainingResultDetailModel>();
            Mapper.CreateMap<ImportTrainingResultDetailModel, TrainingResultDetail>();
            Mapper.CreateMap<FilesHistoryImport, FileHistoryImport>();
        }
        // GET: Home/TrainingResult
        public ActionResult TrainingResults()
        {
            return View();

        }

        [HttpPost]
        public ActionResult GetPage_TrainingResults([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _TrainingResultRepository.Get_TrainingResults(out totalRows, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetPage_TrainingResultDetails([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, long id)
        {
            var requestForm = Request.Form;
            int totalRows;
            var requestParams = DatatableHelper.GetParamsFromRequest(requestModel, requestForm);
            var pageIndex = requestParams.PageIndex;
            var pageSize = requestParams.PageSize;
            var orderBy = requestParams.OrderBy;
            var searchBy = requestParams.SearchBy;

            var data = _TrainingResultDetailRepository.Get_TrainingResultDetails(out totalRows, id, pageIndex, pageSize, orderBy, searchBy);
            return Json(new { draw = requestModel.Draw, recordsTotal = totalRows, recordsFiltered = totalRows, data = data.ToArray() }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult TrainingResult(long? id)
        {
            var classes = _classRepository.Get_AllClassesNotTrainingResult().ToList();
            if (id == null)
            {
                ViewBag.Class = classes;
                var model = new TrainingResultModel()
                {
                    ClassID = 1,
                    IsCreated = false
                };
                model.IsEdit = false;
                var data = Mapper.Map<TrainingResultModel, TrainingResult>(model);
                ViewBag.TrainingResultID = model.TrainingResultID = _TrainingResultRepository.Insert(data);

                return View("_TrainingResult", model);
            }
            else
            {

                var TrainingResult = _TrainingResultRepository.Get_TrainingResultByTrainingResultID((long)id);
                classes.Add(_classRepository.Get_ClassByClassID(TrainingResult.ClassID));
                ViewBag.Class = classes;
                var model = Mapper.Map<TrainingResult, TrainingResultModel>(TrainingResult);
                model.IsEdit = true;
                ViewBag.TrainingResultID = model.TrainingResultID;
                return View("_TrainingResult", model);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("TrainingResult")]
        public ActionResult TrainingResult(TrainingResultModel model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Có lỗi xảy ra. Vui lòng kiểm tra lại");
            try
            {

                var TrainingResult = Mapper.Map<TrainingResultModel, TrainingResult>(model);
                TrainingResult.IsCreated = true;
                _TrainingResultRepository.Update(TrainingResult);
                return this.RedirectToAction("TrainingResults", "TrainingResult");

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ActionName("DeleteTrainingResult")]
        public ActionResult DeleteTrainingResult(List<long> id)
        {
            if (id == null)
                return Json(new { success = false, message = "Bạn chưa chọn bản ghi!" }, JsonRequestBehavior.AllowGet);
            try
            {
                _TrainingResultRepository.Delete(id);
                var message = "Xóa sinh viên thành công!";
                return Json(new { success = true, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sinh viên đã được dùng ở chức năng khác" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult TrainingResultDetail(long? id, long? trainingResultDetailID)
        {

            var TrainingResultDetail = _TrainingResultDetailRepository.Get_TrainingResultDetailByTrainingResultDetailID((long)id);
            var TrainingResult = _TrainingResultRepository.Get_TrainingResultByTrainingResultID(TrainingResultDetail.TrainingResultID);
            ViewBag.ClassStudent = _classStudentRepository.Get_StudentInClassNotInTrainingResult(TrainingResultDetail.TrainingResultID, TrainingResult.ClassID, TrainingResultDetail.TrainingResultDetailID).ToList();

            var model = Mapper.Map<TrainingResultDetail, TrainingResultDetailModel>(TrainingResultDetail);
            model.IsEdit = true;
            return PartialView("_trainingResultPopup", model);
        }
        [HttpGet]
        public ActionResult InsertTrainingResultDetail(long id, long classID)
        {
            var model = new TrainingResultDetailModel();
            model.IsEdit = false;
            model.TrainingResultID = id;
            ViewBag.ClassStudent = _classStudentRepository.Get_StudentInClassNotInTrainingResult(id, classID, 0).ToList();

            return PartialView("_trainingResultPopup", model);

        }

        [HttpGet]
        public ActionResult DownloadTemplate(long classID)
        {
            try
            {
                var excelTemplateFile = OPCPackage.Open(Server.MapPath(@"\Code\TemplateImport\template_diem_sinh_vien.xlsx"));
                var templateWorkbook = new XSSFWorkbook(excelTemplateFile);
                var sheet = templateWorkbook.GetSheet("danh_sach");
                var total = 0;
                var trainingResultDetails = _TrainingResultDetailRepository.GetTrainingResultDetailForExport(classID);
                var datas = Mapper.Map<IEnumerable<ImportTrainingResultDetailModel>>(trainingResultDetails);
                AddObjects(
                    sheet, datas.ToList(),
                    _ => _.FullInforDisplay,
                    _ => _.ScoreFirt,
                    _ => _.ScoreLast,
                    _ => _.ScorePracticeFirst,
                    _ => _.ScorePracticeLast
                );
                var stream = new MemoryStream();
                templateWorkbook.Write(stream);

                var excelTemplateFileNameDownload = $"danh_sach_diem_hoc_sinh_{DateTime.Now:ddMMyyyy}_{DateTime.Now:HHmmss}.xlsx";
                return File(stream.ToArray(), "application/vnd.ms-excel", excelTemplateFileNameDownload);
            }
            catch (Exception e)
            {
                throw e;
            }
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

        #region Tải file mẫu
        private XSSFWorkbook LoadDataToExcel()
        {
            var excelTemplateFile = OPCPackage.Open(Server.MapPath(@"\Code\TemplateImport\template_diem_sinh_vien.xlsx"));
            var templateWorkbook = new XSSFWorkbook(excelTemplateFile);
            var sheetCategory = templateWorkbook.GetSheet("danh_sach");

            //pass word file 
            //string passTemplate = ConfigurationManager.AppSettings.Get("TemplatePass");
            //sheetCategory.ProtectSheet(passTemplate);

            return templateWorkbook;
        }
        #endregion




        #region Import
        [HttpPost]
        public ActionResult ImportExcelFile(long classID, long trainingResultID)
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

                var trainingResultDetails = _TrainingResultDetailRepository.GetTrainingResultDetailForExport(classID);


                int totalRow = sheet.LastRowNum;

                var invalidTrainingResultDetailModels = new List<ImportTrainingResultDetailModel>();
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
                    var entity = ProcessExcelRow(sheet, rowIndex, trainingResultDetails.ToArray(), classID, trainingResultID);

                    if (entity != null)
                    {
                        var success = ImportStudents(entity, invalidTrainingResultDetailModels, classID);
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
                if (invalidTrainingResultDetailModels.Any())
                {
                    var fileName = $"{User.Identity.Name}_import_diem_hoc_sinh_{DateTime.Now.ToString("ddMMyyyy")}_{DateTime.Now.ToString("HHmmss")}_error.xlsx";
                    SaveInvalidRowToDisk(invalidTrainingResultDetailModels, fileName);

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
        private ImportTrainingResultDetailModel ProcessExcelRow(ISheet worksheet, int row, TrainingResultDetail[] trainingResultDetails, long classID, long trainingResultID)
        {
            var upto255Characters = 255;
            var exceptionMessage = new StringBuilder();
            var model = new ImportTrainingResultDetailModel();

            try
            {
                model.FullInfor = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 0, "Sinh viên", true, upto255Characters, exceptionMessage);
                if (!string.IsNullOrEmpty(model.FullInfor))
                {
                    var studentID = model.FullInfor.Split(';')[0];
                    var trainingResultDetail = trainingResultDetails.FirstOrDefault(x => x.StudentID == Convert.ToInt64(studentID));
                    if (trainingResultDetail == null)
                        exceptionMessage.Append($"Sinh viên không tồn tại trong lớp; ");
                    else
                    {
                        model.StudentID = trainingResultDetail.StudentID;
                    }
                }
                var scoreFirst = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 1, "Điểm lý thuyết lần thứ nhất", true, upto255Characters, exceptionMessage);
                model.ScoreFirt = !string.IsNullOrEmpty(scoreFirst) ? Convert.ToDecimal(scoreFirst) : default(decimal?);

                var scoreSecond = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 2, "Điểm lý thuyết lần thứ 2", false, upto255Characters, exceptionMessage);
                model.ScoreLast = !string.IsNullOrEmpty(scoreSecond) ? Convert.ToDecimal(scoreSecond) : default(decimal?);

                var scorePracticeFirst = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 3, "Điểm thực hành lần thứ nhất", true, upto255Characters, exceptionMessage);
                model.ScorePracticeFirst = !string.IsNullOrEmpty(scorePracticeFirst) ? Convert.ToDecimal(scorePracticeFirst) : default(decimal?);

                var scorePracticeLast = NpoiImportHelper.GetStringCellValueFromRow(worksheet, row, 4, "Điểm thực hành lần thứ 2", false, upto255Characters, exceptionMessage);
                model.ScorePracticeLast = !string.IsNullOrEmpty(scorePracticeLast) ? Convert.ToDecimal(scorePracticeLast) : default(decimal?);
                model.ClassID = classID;
                model.TrainingResultID = trainingResultID;
                model.Exception = exceptionMessage.ToString();
                return model;
            }
            catch (Exception exception)
            {
                model.Exception = exception.Message;
                return model;
            }
        }
        private bool ImportStudents(ImportTrainingResultDetailModel item, List<ImportTrainingResultDetailModel> invalidTrainingResultDetailModels, long classId)
        {
            if (item.CanBeImported())
            {
                try
                {
                    var trainingResultDetail = Mapper.Map<TrainingResultDetail>(item);
                    _TrainingResultDetailRepository.Import(trainingResultDetail);
                    return true;
                }
                catch (Exception exception)
                {
                    item.Exception = exception.Message;
                    invalidTrainingResultDetailModels.Add(item);
                    return false;
                }
            }
            else
            {
                invalidTrainingResultDetailModels.Add(item);
                return false;
            }
        }
        private bool IsValidTemplate(IRow row)
        {
            try
            {
                var validcellNameArray = new List<string>
                {
                    "Sinh Viên",
                    "Điểm lý thuyết lần đầu",
                    "Điểm lý thuyết lần 2",
                    "Điểm thực hành lần đầu",
                    "Điểm thực hành lần 2",
                };

                var needToCheckArray = new List<string>();
                for (int i = 0; i <= 4; i++)
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
        private void SaveInvalidRowToDisk(List<ImportTrainingResultDetailModel> invalidTrainingResultDetailModels, string fileName)
        {
            const string importHistoryFolder = "~/FileHistoryImport";
            Directory.CreateDirectory(Server.MapPath(importHistoryFolder));
            string path = Path.Combine(Server.MapPath(importHistoryFolder), fileName);

            var filesHistoryImport = new FilesHistoryImport()
            {
                ControllerName = nameof(TrainingResult),
                FileName = fileName,
                TypeUser = (byte)TypeUser,
                UserID = UserID != -1 ? UserID : TeacherID,
            };
            var model = Mapper.Map<FileHistoryImport>(filesHistoryImport);
            _fileHistoryImportRepository.Insert(model);

            var workbook = LoadDataToExcel();
            var sheet = workbook.GetSheetAt(0);

            var errorCell = sheet.GetRow(1).CreateCell(5);
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
                invalidTrainingResultDetailModels,
                _ => _.FullInfor,
                _ => _.ScoreFirt,
                _ => _.ScoreLast,
                _ => _.ScorePracticeFirst,
                _ => _.ScorePracticeLast,
                _ => _.Exception
            );

            sheet.SetColumnWidth(19, 20000);

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }

        protected void AddSheetContent(ISheet sheet, int startRowIndex, List<ImportTrainingResultDetailModel> items, params Func<ImportTrainingResultDetailModel, object>[] propertySelectors)
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