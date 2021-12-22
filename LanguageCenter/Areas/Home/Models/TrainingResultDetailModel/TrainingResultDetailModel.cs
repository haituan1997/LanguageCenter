using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Models.TrainingResultDetailModel
{
    public class TrainingResultDetailModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }

        public long TrainingResultDetailID { get; set; }
        public long TrainingResultID { get; set; }
        public long StudentID { get; set; }
        public string LastName { get; set; }
        public string FirtName { get; set; }
        public decimal ScoreFirt { get; set; } 
        public decimal? ScoreLast { get; set; }
        public decimal? ScorePracticeFirst { get; set; }
        public decimal? ScorePracticeLast { get; set; }
        public string ClassName { get; set; }
        public string ClassID { get; set; }
    }
}