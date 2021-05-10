using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Helper.StaticData
{
    /// <summary>
    /// Class StaticDataHelper.
    /// </summary>
    public static class StaticDataHelper
    {
        /// <summary>
        /// Gets or sets the yes no.
        /// </summary>
        /// <value>The yes no.</value>
        public static List<EnumModel> PaymentMethod { get; set; }
        public static List<EnumModel> StatusPaymentMethod { get; set; }

         

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void InitializeData()
        {
            var enums = new Enumerator.Enumerator();

            PaymentMethod = new List<EnumModel>();

            foreach (Enumerator.Enumerator.PaymentMethod paymentMethod in Enum.GetValues(typeof(Enumerator.Enumerator.PaymentMethod)))
            {
                PaymentMethod.Add(new EnumModel { Key = (int)paymentMethod, Value = enums.GetEnumDescription(paymentMethod) });
            }

            StatusPaymentMethod = new List<EnumModel>();

            foreach (Enumerator.Enumerator.StatusPaymentMethod statusPaymentMethod in Enum.GetValues(typeof(Enumerator.Enumerator.StatusPaymentMethod)))
            {
                StatusPaymentMethod.Add(new EnumModel { Key = (int)statusPaymentMethod, Value = enums.GetEnumDescription(statusPaymentMethod) });
            }

        }
    }
}