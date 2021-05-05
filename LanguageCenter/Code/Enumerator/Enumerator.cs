using System;
using System.ComponentModel;

namespace Code.Enumerator
{
    public class Enumerator
    {
       
        public enum PaymentMethod
        {
            [Description("Trực tiếp")]
            Type1 = 1,
            [Description("Chuyển khoản")]
            Type2 = 2
        }
        public enum StatusPaymentMethod
        {
            [Description("Đã hoàn tất")]
            Type1 = 1,
            [Description("Chưa hoàn tất")]
            Type2 = 2
        }
        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}