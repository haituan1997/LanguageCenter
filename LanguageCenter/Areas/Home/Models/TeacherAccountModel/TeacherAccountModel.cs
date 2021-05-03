using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.TeacherAccountModel
{
    public class TeacherAccountModel
    {
        public long TeacherAccountID { get; set; }

        public bool? IsActive { get; set; }

        public string UserLogin { get; set; }

        public string PassWordLogin { get; set; }

        public long TeacherID { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public bool IsEdit { get; set; }
    }
}