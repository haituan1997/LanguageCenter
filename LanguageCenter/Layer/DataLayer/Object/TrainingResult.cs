using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class TrainingResult
    { 
        public long TrainingResultID { get; set; } 
        public long ClassID { get; set; } 

        public string ClassName { get; set; }

        public bool IsCreated { get; set; }




    }

}