using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace LanguageCenter.Code.Helper.NpoiHelper
{
    public static class NpoiImportHelper
    {
        private static CultureInfo _cultureInfo => new CultureInfo("vi-VN");
        private static Regex _fullDateRegex => new Regex(@"^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$");
        private static Regex _monthYearRegex => new Regex(@"^((0[1-9])|(1[0-2]))\/(\d{4})$");
        private static Regex _yearRegex => new Regex(@"^\d{4}$");

        public static string GetStringCellValueFromRow(ISheet worksheet, int row, int column, string columnName, bool isRequired, int? maxLength, StringBuilder exceptionMessage)
        {
            var cell = worksheet.GetRow(row).Cells[column];
            cell.SetCellType(CellType.String);
            var cellValue = cell.StringCellValue;
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                if (isRequired)
                    exceptionMessage.Append($"{ columnName } không được phép để trống; ");
            }

            if (maxLength != null && cellValue.Length > maxLength)
                exceptionMessage.Append($"{ columnName } không được phép vượt quá { maxLength.Value }; ");

            return cellValue;
        }

        public static string GetDateFormatCellValueFromRow(ISheet worksheet, int row, int column, string columnName, bool isRequired, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.GetRow(row).Cells[column].StringCellValue;
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                if (isRequired)
                    exceptionMessage.Append($"{ columnName } không được phép để trống; ");

                return null;
            }

            var isFullDateRegexValid = _fullDateRegex.IsMatch(cellValue);
            var isMonthYearRegexValid = _monthYearRegex.IsMatch(cellValue);
            var isYearRegexValid = _yearRegex.Match(cellValue);

            if (isFullDateRegexValid)
            {
                DateTime validDate;
                if (!DateTime.TryParseExact(cellValue, "dd/MM/yyyy", _cultureInfo, DateTimeStyles.None, out validDate))
                    exceptionMessage.Append($"{ columnName } không tồn tại; ");

                if (validDate < new DateTime(1753, 1, 1) || validDate > DateTime.Now)
                    exceptionMessage.Append($"{ columnName } không được nhỏ hơn 01/01/1753 và không được lớn hơn ngày hiện tại; ");
            }
            else if (isMonthYearRegexValid)
            {
                var yearPart = int.Parse(cellValue.Substring(cellValue.Trim().IndexOf('/') + 1));
                if (yearPart > DateTime.Now.Year || yearPart < 1753)
                    exceptionMessage.Append($"{ columnName } không đúng định dạng; ");
            }
            else if (isYearRegexValid.Success)
            {
                if (Convert.ToInt32(cellValue) > DateTime.Now.Year || Convert.ToInt32(cellValue) < 1753)
                    exceptionMessage.Append($"{ columnName } không được nhỏ hơn 01/01/1753 và không được lớn hơn ngày hiện tại; ");
            }
            else
            {
                exceptionMessage.Append($"Định dạng { columnName.FirstCharToLowerCase() } không đúng yêu cầu nhập lại; ");
            }

            return cellValue;
        }
        public static string FirstCharToLowerCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;

            return char.ToLower(str[0]) + str.Substring(1);
        }
        public static byte GetByteValueFromString(ISheet worksheet, int row, int column, string columnName, Dictionary<int, string> keyValues, StringBuilder exceptionMessage)
        {
            byte value = 0;
            var cellValue = worksheet.GetRow(row).Cells[column].StringCellValue;

            var result = keyValues.FirstOrDefault(x => x.Value.Equals(cellValue, StringComparison.OrdinalIgnoreCase));
            if (result.Equals(default(KeyValuePair<int, string>)))
                exceptionMessage.Append($"{ columnName } không tồn tại trong hệ thống;");
            else
                value = Convert.ToByte(result.Key);

            return value;
        }

        public static int GetIntValueFromString(ISheet worksheet, int row, int column, string columnName, Dictionary<int, string> keyValues, StringBuilder exceptionMessage)
        {
            int value = 0;
            var cellValue = worksheet.GetRow(row).Cells[column].StringCellValue;

            var result = keyValues.FirstOrDefault(x => x.Value.Equals(cellValue, StringComparison.OrdinalIgnoreCase));
            if (result.Equals(default(KeyValuePair<int, string>)))
                exceptionMessage.Append($"{ columnName } không tồn tại trong hệ thống;");
            else
                value = Convert.ToInt32(result.Key);

            return value;
        }

        public static long GetLongValueFromStringPart(ISheet worksheet, int row, int column, string columnName, List<int> keys, StringBuilder exceptionMessage)
        {
            long value = 0;
            var cellValue = worksheet.GetRow(row).Cells[column].StringCellValue;
            if (cellValue.IndexOf(';') < 0)
            {
                exceptionMessage.Append($"{ columnName } không đúng định dạng; ");
            }
            else
            {
                var result = long.TryParse(cellValue.Substring(0, cellValue.IndexOf(';')), out value);
                if (result)
                {
                    if (!keys.Any(x => x == value))
                        exceptionMessage.Append($"{ columnName } không tồn tại trong hệ thống; ");
                }
            }


            return value;
        }

        /// <summary>
        /// Lấy giá trị của dropdownlist đang phụ thuộc vào dropdownlist khác
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="columnName"></param>
        /// <param name="keyValues"></param>
        /// <param name="dependColumnName"></param>
        /// <param name="dependColumnValue"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        public static long GetLongValueFromStringPartCascade(ISheet worksheet, int row, int column, string columnName, Dictionary<long, long> keyValues, string dependColumnName, long dependColumnValue, StringBuilder exceptionMessage)
        {
            long value = 0;
            var cellValue = worksheet.GetRow(row).Cells[column].StringCellValue;
            if (cellValue.IndexOf(';') < 0)
            {
                exceptionMessage.Append($"{ columnName } không đúng định dạng; ");
            }
            else
            {
                var indexOfComma = cellValue.IndexOf(';') + 1;
                var lastIndexOfComma = cellValue.LastIndexOf(';');
                var result = long.TryParse(cellValue.Substring(indexOfComma, lastIndexOfComma - indexOfComma), out value);
                if (result)
                {
                    var keyExists = keyValues.FirstOrDefault(x => x.Key == value);
                    if (keyExists.Equals(default(KeyValuePair<int, long>)))
                    {
                        exceptionMessage.Append($"{ columnName } không tồn tại trong hệ thống; ");
                    }
                    else if (keyExists.Value != dependColumnValue)
                    {
                        exceptionMessage.Append($"{ columnName } không có trong { dependColumnName }; ");
                    }
                }
            }

            return value;
        }
        public static bool CheckCompareText(string fieldImportText, string fieldCompareText)
        {
            byte[] fieldImport = Encoding.ASCII.GetBytes(fieldImportText.Trim());
            byte[] fieldCompare = Encoding.ASCII.GetBytes(fieldCompareText.Trim());
            if (string.Join(",", fieldImport) == string.Join(",", fieldCompare))
            {
                return true;
            }
            return false;
        }
    }
}