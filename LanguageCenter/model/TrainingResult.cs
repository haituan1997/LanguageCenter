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
    
    public partial class TrainingResult
    {
        public TrainingResult()
        {
            this.TrainingResultDetails = new HashSet<TrainingResultDetail>();
        }
    
        public long TrainingResultID { get; set; }
        public long ClassID { get; set; }
    
        public virtual Class Class { get; set; }
        public virtual ICollection<TrainingResultDetail> TrainingResultDetails { get; set; }
    }
}
