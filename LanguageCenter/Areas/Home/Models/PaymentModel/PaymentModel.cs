using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Models.PaymentModel
{
    public class PaymentModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }

        public long PaymentID { get; set; }

        public string FirtName { get; set; }

        public string LastName { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public short PaymentMethodID { get; set; }
        public short Status { get; set; }
        public long StudentID { get; set; }
        public long ClassID { get; set; }
        public string ClassName { get; set; }
    }
}