using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class Teacher
    {
        public bool IsEdit { get; set; }

        public long TeacherID { get; set; }

        public string AvatarUrl { get; set; }

        public string FirtName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string NumberPhone { get; set; }

        public string Description { get; set; }

    }

}