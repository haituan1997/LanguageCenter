using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.ClassStudent
{
    public class ClassStudentModel
    {
        public long ClassStudentID { get; set; }

        public long ClassID { get; set; }

        public long StudentID { get; set; }
        public string Title { get; set; }
    }
}