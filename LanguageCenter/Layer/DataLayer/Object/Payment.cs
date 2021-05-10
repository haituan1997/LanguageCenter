using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class Payment
    { 
        public long PaymentID { get; set; } 

        public string FirtName { get; set; }
        public string FullName { get; set; }

        public string LastName { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public short PaymentMethodID { get; set; }
        public string PaymentMethodName { get; set; }
        public short Status { get; set; }
        public long StudentID { get; set; }
        public long ClassID { get; set; }
        public string ClassName { get; set; } 


    }

}