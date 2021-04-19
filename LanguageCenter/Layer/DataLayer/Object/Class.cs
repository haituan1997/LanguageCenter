using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.DataLayer.Object
{
    public class Class
    {
        public long ClassID { get; set; }

        public string ClassName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Price { get; set; }

        public long TeacherID { get; set; }

        public long CourseID { get; set; }

    }
}