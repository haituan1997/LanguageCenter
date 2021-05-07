using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class ClassWeekDay
    {
        public long ClassWeekDayID { get; set; }

        public long? ClassID { get; set; }

        public DateTime ClassWeekDayTime { get; set; }
        public string ClassWeekDayName { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

    }
}