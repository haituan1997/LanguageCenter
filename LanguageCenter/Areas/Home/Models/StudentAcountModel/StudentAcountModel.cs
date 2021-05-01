using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.StudentAccountModel
{
    public class StudentAccountModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }
        public long StudentAccountID { get; set; }
        public long StudentID { get; set; }

        public string UserLogin { get; set; }

        public string PassWordLogin { get; set; }
        public bool IsActive { get; set; }
    }
}