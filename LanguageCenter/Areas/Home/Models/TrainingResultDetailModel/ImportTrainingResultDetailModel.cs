using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.TrainingResultDetailModel
{
    public class ImportTrainingResultDetailModel
    {
        public long TrainingResultDetailID { get; set; }
        public long TrainingResultID { get; set; }
        public long StudentID { get; set; }
        public string LastName { get; set; }
        public string FirtName { get; set; }
        public string FullInfor { get; set; }
        public string FullInforDisplay
        {
            get
            {
                return $"{StudentID};{FirtName} {LastName}";
            }
        }
        public decimal? ScoreFirt { get; set; }
        public decimal? ScoreLast { get; set; }
        public decimal? ScorePracticeFirst { get; set; }
        public decimal? ScorePracticeLast { get; set; }
        public string ClassName { get; set; }
        public long ClassID { get; set; }
        public string Exception { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}