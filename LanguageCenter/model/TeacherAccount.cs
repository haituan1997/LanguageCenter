//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LanguageCenter.model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TeacherAccount
    {
        public long TeacherAccountID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string UserLogin { get; set; }
        public string PassWordLogin { get; set; }
        public long TeacherID { get; set; }
    
        public virtual Teacher Teacher { get; set; }
    }
}
