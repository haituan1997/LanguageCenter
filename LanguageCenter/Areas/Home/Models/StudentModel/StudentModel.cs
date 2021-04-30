using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.StudentModel
{
    public class StudentModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }

        public long StudentID { get; set; }

        public string AvatarUrl { get; set; }

        public string FirtName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Description { get; set; }
    }
}