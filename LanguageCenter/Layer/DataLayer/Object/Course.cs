using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class Course
    {
        public long CourseID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long LanguageID { get; set; }

        public long LevelID { get; set; }

        public long CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string LanguageName { get; set; }

        public string LevelName { get; set; }

        public string LevelCode { get; set; }


    }
}