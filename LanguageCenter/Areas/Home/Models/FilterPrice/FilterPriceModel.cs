using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.FilterPrice
{
    public class FilterPriceModel
    {
        public long PaymentID { get; set; }

        public long StudentID { get; set; }
        public long ClassID { get; set; }
        public long CourseID { get; set; }
        public string ClassName { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string ToTalAmount { get; set; }
    }
}