using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.Class
{
    public class NewClassModel
    {
        public long? ClassID { get; set; }

        public string ClassName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Price { get; set; }

        public long? TeacherID { get; set; }

        public long? CourseID { get; set; }
        
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string CourseName { get; set; }
        public bool IsEdit { get; set; }
        public bool IsCreated { get; set; }
        public string Title { get; set; }
    }
}