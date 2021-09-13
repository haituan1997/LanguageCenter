using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Code.Helper.NpoiHelper
{
    public static class NpoiExtension
    {
        /// <summary>
        /// Creates the drop down using cell reference.
        /// </summary>
        /// <param name="constraintValues">The constraint values.</param>
        /// <param name="workbook">The workbook.</param>
        /// <param name="sheetToSetValue">The sheet to set value.</param>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <param name="rangeToSetValue">The range to set value.</param>
        //public static void CreateDropDownUsingCellReference(this IEnumerable constraintValues, IWorkbook workbook, ISheet sheetToSetValue, string constraintName, string rangeToSetValue)
        //{
        //    try
        //    {
        //        var indexArray = rangeToSetValue.Split('-');
        //        if (indexArray.Length <= 0) return;

        //        var headerNameOfConstraint = indexArray[0];
        //        var rowIndexOfConstraintToBeginSetValue = int.Parse(indexArray[1]); //index start from 0
        //        var columnIndex = CellReference.ConvertColStringToIndex(headerNameOfConstraint);
        //        var referenceNamedCell = (XSSFName)workbook.CreateName();
        //        referenceNamedCell.NameName = constraintName;

        //        //Creating Cell and Assigning Values from CSVListOfValues;
        //        var index = rowIndexOfConstraintToBeginSetValue - 1;
        //        foreach (var value in constraintValues)
        //        {
        //            if (value == null) continue;
        //            var row = sheetToSetValue.GetRow(index);
        //            var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
        //            cell.SetCellValue(value.ToString());
        //            index++;
        //        }

        //        //Assigning the Reference for sheet 0 With Cell Range, where list items iscopied
        //        if (index <= rowIndexOfConstraintToBeginSetValue)
        //        {
        //            var referenceIndex = $"{sheetToSetValue.SheetName}!${headerNameOfConstraint}${rowIndexOfConstraintToBeginSetValue}:${headerNameOfConstraint}${rowIndexOfConstraintToBeginSetValue}";
        //            referenceNamedCell.RefersToFormula = referenceIndex;
        //        }
        //        else
        //        {
        //            var referenceIndex = $"{sheetToSetValue.SheetName}!${headerNameOfConstraint}${rowIndexOfConstraintToBeginSetValue}:${headerNameOfConstraint}${index}";
        //            referenceNamedCell.RefersToFormula = referenceIndex;
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //    }
        //}

        public static void CreateDropDownUsingCellReference(this IEnumerable constraintValues, IWorkbook workbook, ISheet sheetToSetValue, string constraintName, string rangeToSetValue)
        {
            try
            {
                var indexArray = rangeToSetValue.Split('-');
                if (indexArray.Length <= 0) return;

                var headerNameOfConstraint = indexArray[0];
                var rowIndexOfConstraintToBeginSetValue = int.Parse(indexArray[1]); //index start from 0
                var columnIndex = CellReference.ConvertColStringToIndex(headerNameOfConstraint);
                var referenceNamedCell = (XSSFName)workbook.CreateName();
                referenceNamedCell.NameName = constraintName;

                //Creating Cell and Assigning Values from CSVListOfValues;
                var index = rowIndexOfConstraintToBeginSetValue - 1;
                foreach (var value in constraintValues)
                {
                    if (value == null) continue;

                    // QuanTN - 24/12/2019: Code cũ này sẽ bị lỗi khi sheet danh_muc_v1 chỉ có duy nhất 1 danh mục 
                    // ->>> var row = sheetToSetValue.GetRow(index) -> sẽ null
                    var row = sheetToSetValue.GetRow(index);
                    if (row == null)
                    {
                        var newRow = sheetToSetValue.CreateRow(index);
                        var cell = newRow.CreateCell(columnIndex);
                        cell.SetCellValue(value.ToString());
                    }
                    else
                    {
                        var cell = row.GetCell(columnIndex) ?? row.CreateCell(columnIndex);
                        cell.SetCellValue(value.ToString());
                    }

                    index++;
                }

                //Assigning the Reference for sheet 0 With Cell Range, where list items iscopied
                if (index <= rowIndexOfConstraintToBeginSetValue)
                {
                    var referenceIndex = $"{sheetToSetValue.SheetName}!${headerNameOfConstraint}${rowIndexOfConstraintToBeginSetValue}:${headerNameOfConstraint}${rowIndexOfConstraintToBeginSetValue}";
                    referenceNamedCell.RefersToFormula = referenceIndex;
                }
                else
                {
                    var referenceIndex = $"{sheetToSetValue.SheetName}!${headerNameOfConstraint}${rowIndexOfConstraintToBeginSetValue}:${headerNameOfConstraint}${index}";
                    referenceNamedCell.RefersToFormula = referenceIndex;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}