using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.TeacherModel
{
    public class TeacherModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }

        public long TeacherID { get; set; }
        
        public string AvatarUrl { get; set; }
        [Required]
        public string FirtName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        public string NumberPhone { get; set; }

        public string Description { get; set; }
    }
}