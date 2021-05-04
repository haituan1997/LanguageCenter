using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.Course
{
    public class CourseModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }
        public long CourseID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long LanguageID { get; set; }

        public long LevelID { get; set; }

        public long CategoryID { get; set; }
    }
}