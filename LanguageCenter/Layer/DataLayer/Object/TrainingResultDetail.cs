using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class TrainingResultDetail
    {

        public long TrainingResultDetailID { get; set; }
        public long TrainingResultID { get; set; }
        public long StudentID { get; set; }
        public string LastName { get; set; }
        public string FirtName { get; set; }
        public decimal ScoreFirt { get; set; }
        public decimal ScoreLast { get; set; }


    }

}