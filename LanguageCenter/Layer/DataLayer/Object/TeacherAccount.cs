using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class TeacherAccount
    {
        public long TeacherAccountID { get; set; }

        public bool? IsActive { get; set; }

        public string UserLogin { get; set; }

        public string PassWordLogin { get; set; }

        public long TeacherID { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }

    }

}