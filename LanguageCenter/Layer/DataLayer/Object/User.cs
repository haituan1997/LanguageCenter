using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.DataLayer.Object
{
    public class User
    {
        public long UserID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
    }
}