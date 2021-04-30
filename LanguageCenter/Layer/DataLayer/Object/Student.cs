using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class Student
    { 
        public long StudentID { get; set; } 

        public string FirtName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CurrentAddress { get; set; }
        public string CityName { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }

}