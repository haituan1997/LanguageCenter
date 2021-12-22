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

        public string FirtName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CurrentAddress { get; set; }
        public string CityName { get; set; }
        public DateTime  DateOfBirth { get; set; }
        public string  DateOfBirthBackUp { get; set; }
        public string UserLogin { get; set; }
        public string PassWordLogin { get; set; }
    }
}