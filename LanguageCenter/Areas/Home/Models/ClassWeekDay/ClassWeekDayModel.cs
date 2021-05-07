using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.ClassWeekDay
{
    public class ClassWeekDayModel
    {
        public long ClassWeekDayID { get; set; }

        public long? ClassID { get; set; }

        public DateTime? ClassWeekDayTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
        public string Title { get; set; }
        public bool IsEdit { get; set; }
    }
}