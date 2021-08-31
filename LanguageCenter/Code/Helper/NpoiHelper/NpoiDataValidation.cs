using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Code.Helper.NpoiHelper
{
    public class NpoiDataValidation
    {
        #region Properties

        public int NumberOfRowLimit { get; set; }
        public int RowIndexToStart { get; set; }
        public string TargetFirstColumn { get; set; }
        public string TargetLastColumn { get; set; }
        public ISheet SheetDataImport { get; set; }
        public string ConstraintName { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NpoiDataValidation"/> class.
        /// </summary>
        public NpoiDataValidation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpoiDataValidation"/> class.
        /// </summary>
        /// <param name="sheetDataImport">The sheet data import.</param>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <param name="numberOfRowLimit">The number of row limit.</param>
        /// <param name="rowIndexToStart">The row index to start.</param>
        /// <param name="targetFirstColumn">The target first column.</param>
        /// <param name="targetLastColumn">The target last column.</param>
        public NpoiDataValidation(ISheet sheetDataImport, string constraintName, int numberOfRowLimit, int rowIndexToStart, string targetFirstColumn, string targetLastColumn)
        {
            SheetDataImport = sheetDataImport;
            ConstraintName = constraintName;
            NumberOfRowLimit = numberOfRowLimit;
            RowIndexToStart = rowIndexToStart;
            TargetFirstColumn = targetFirstColumn;
            TargetLastColumn = targetLastColumn;

            var targetFirstColumnIndex = CellReference.ConvertColStringToIndex(TargetFirstColumn);
            var targetLastColumnIndex = CellReference.ConvertColStringToIndex(TargetLastColumn);

            _validationAddressList = new CellRangeAddressList(rowIndexToStart, numberOfRowLimit, targetFirstColumnIndex, targetLastColumnIndex);
            _validationHelper = new XSSFDataValidationHelper((XSSFSheet)sheetDataImport);
        }

        /// <summary>
        /// Adds the validation.
        /// </summary>
        public void AddValidation()
        {
            var dataValidConstraint = (XSSFDataValidationConstraint)_validationHelper.CreateFormulaListConstraint("=" + ConstraintName);
            dataValidConstraint.Validate();

            var dataValidation = (XSSFDataValidation)_validationHelper.CreateValidation(dataValidConstraint, _validationAddressList);
            dataValidation.ShowErrorBox = true;
            dataValidation.SuppressDropDownArrow = true;
            dataValidation.ErrorStyle = 0;

            var titleErrorBox = string.IsNullOrEmpty(_titleErrorBox) ? TitleErrorBoxConst : _titleErrorBox;
            var contentErrorBox = string.IsNullOrEmpty(_contentErrorBox) ? ContentErrorBoxConst : _contentErrorBox;
            var titlePromptBox = string.IsNullOrEmpty(_titlePromptBox) ? TitlePromptBoxConst : _titlePromptBox;
            var contentPromptBox = string.IsNullOrEmpty(_contentPromptBox) ? ContentPromptBoxConst : _contentPromptBox;

            dataValidation.CreateErrorBox(titleErrorBox, contentErrorBox);
            dataValidation.ShowErrorBox = true;
            dataValidation.CreatePromptBox(titlePromptBox, contentPromptBox);
            dataValidation.ShowPromptBox = true;
            SheetDataImport.AddValidationData(dataValidation);
        }

        /// <summary>
        /// Initializes the validation address list.
        /// </summary>
        public void InitValidationAddressList()
        {
            if (string.IsNullOrEmpty(TargetFirstColumn)) throw new Exception();
            if (string.IsNullOrEmpty(TargetLastColumn)) throw new Exception();

            var targetFirstColumnIndex = CellReference.ConvertColStringToIndex(TargetFirstColumn);
            var targetLastColumnIndex = CellReference.ConvertColStringToIndex(TargetLastColumn);

            _validationAddressList = new CellRangeAddressList(RowIndexToStart, NumberOfRowLimit, targetFirstColumnIndex, targetLastColumnIndex);
        }

        /// <summary>
        /// Initializes the validation helpder.
        /// </summary>
        public void InitValidationHelpder()
        {
            _validationHelper = new XSSFDataValidationHelper((XSSFSheet)SheetDataImport);
        }

        public void InitValidationBox(string titleErrorBox, string contentErrorBox, string titlePromptBox, string contentPromptBox)
        {
            _titleErrorBox = titleErrorBox;
            _contentErrorBox = contentErrorBox;
            _titlePromptBox = titlePromptBox;
            _contentPromptBox = contentPromptBox;
        }

        private XSSFDataValidationHelper _validationHelper;
        private CellRangeAddressList _validationAddressList;
        private string _titleErrorBox;
        private string _contentErrorBox;
        private string _titlePromptBox;
        private string _contentPromptBox;

        private const string TitleErrorBoxConst = "Invalid Value";
        private const string ContentErrorBoxConst = "Select Valid!";
        private const string TitlePromptBoxConst = "Data Validation";
        private const string ContentPromptBoxConst = "Select data!";
    }
}