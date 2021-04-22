using Amazon.IdentityManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageCenter.Models
{
    public class UserModel
    { 
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }

    }
}