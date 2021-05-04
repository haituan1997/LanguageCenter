using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models
{
    public class RegisterModel
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "UserLogin")]
        public string UserLogin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "PassWordLogin")]
        public string PassWordLogin { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "re_pass")]
        public string re_pass { get; set; }

        [Required]
        [Display(Name = "FirtName")]
        public string FirtName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "CurrentAddress")]
        public string CurrentAddress { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

    }
}