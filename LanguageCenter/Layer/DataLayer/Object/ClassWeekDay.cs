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

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

    }
}