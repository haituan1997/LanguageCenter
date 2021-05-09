using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Models.TrainingResultModel
{
    public class TrainingResultModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }

        public long TrainingResultID { get; set; }
        public long ClassID { get; set; }

        public string ClassName { get; set; }
        public bool? IsCreated { get; set; }
    }
}