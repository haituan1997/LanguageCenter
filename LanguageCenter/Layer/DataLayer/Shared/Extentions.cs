using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.DataLayer.Shared
{
    public static class Extensions
    {
        /// <summary>
        /// Ases the long.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultLong">The default long.</param>
        /// <returns></returns>
        public static long AsLong(this object item, long defaultLong = default(long))
        {
            if (item == null)
                return defaultLong;

            long result;
            if (!long.TryParse(item.ToString(), out result))
                return defaultLong;

            return result;
        }

        /// <summary>
        /// Transform object into Identity data type (integer).
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <param name="defaultId">Optional default value is -1.</param>
        /// <returns>Identity value.</returns>
        public static int AsId(this object item, int defaultId = -1)
        {
            if (item == null)
                return defaultId;

            int result;
            if (!int.TryParse(item.ToString(), out result))
                return defaultId;

            return result;
        }

        /// <summary>
        /// Ases the int.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultInt">The default int.</param>
        /// <returns></returns>
        public static int AsInt(this object item, int defaultInt = default(int))
        {
            if (item == null)
                return defaultInt;

            int result;
            if (!int.TryParse(item.ToString(), out result))
                return defaultInt;

            return result;
        }

        /// <summary>
        /// Ases the byte.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultByte">The default byte.</param>
        /// <returns></returns>
        public static byte AsByte(this object item, byte defaultByte = default(byte))
        {
            if (item == null)
                return defaultByte;

            byte result;
            if (!byte.TryParse(item.ToString(), out result))
                return defaultByte;

            return result;
        }

        public static byte? AsByteForNull(this object item, byte? defaultByte = null)
        {
            if (item == null)
                return defaultByte;

            byte result;
            if (!byte.TryParse(item.ToString(), out result))
                return defaultByte;

            return result;
        }

        /// <summary>
        /// Ases the int for null.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultInt">The default int.</param>
        /// <returns></returns>
        public static int? AsIntForNull(this object item, int? defaultInt = null)
        {
            if (item == null)
                return defaultInt;

            int result;
            if (!int.TryParse(item.ToString(), out result))
                return defaultInt;

            return result;
        }

        public static long? AsLongForNull(this object item, long? defaultLong = null)
        {
            if (item == null)
                return defaultLong;

            long result;
            if (!long.TryParse(item.ToString(), out result))
                return defaultLong;

            return result;
        }

        /// <summary>
        /// Ases the short.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultShort">The default short.</param>
        /// <returns></returns>
        public static short AsShort(this object item, short defaultShort = default(short))
        {
            if (item == null)
                return defaultShort;

            short result;
            if (!short.TryParse(item.ToString(), out result))
                return defaultShort;

            return result;
        }

        /// <summary>
        /// Ases the int for null.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultShort">The default int.</param>
        /// <returns></returns>
        public static short? AsShortForNull(this object item, short? defaultShort = null)
        {
            if (item == null)
                return defaultShort;

            short result;
            if (!short.TryParse(item.ToString(), out result))
                return defaultShort;

            return result;
        }

        /// <summary>
        /// Ases the double.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultDouble">The default double.</param>
        /// <returns></returns>
        public static double AsDouble(this object item, double defaultDouble = default(double))
        {
            if (item == null)
                return defaultDouble;

            double result;
            if (!double.TryParse(item.ToString(), out result))
                return defaultDouble;

            return result;
        }

        public static double? AsDoubleForNull(this object item, double? defaultDouble = null)
        {
            if (item == null)
                return defaultDouble;

            double result;
            if (!double.TryParse(item.ToString(), out result))
                return defaultDouble;

            return result;
        }

        /// <summary>
        /// Ases the decimal.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultDecimal">The default decimal.</param>
        /// <returns></returns>
        public static decimal AsDecimal(this object item, decimal defaultDecimal = default(decimal))
        {
            if (item == null)
                return defaultDecimal;

            decimal result;
            if (!decimal.TryParse(item.ToString(), out result))
                return defaultDecimal;

            return result;
        }

        /// <summary>
        /// Ases the decimal for null.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultDecimal">The default decimal.</param>
        /// <returns></returns>
        public static decimal? AsDecimalForNull(this object item, decimal? defaultDecimal = null)
        {
            if (item == null)
                return defaultDecimal;

            decimal result;
            if (!decimal.TryParse(item.ToString(), out result))
                return defaultDecimal;

            return result;
        }

        /// <summary>
        /// Ases the float.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultFloat">The default float.</param>
        /// <returns></returns>
        public static float AsFloat(this object item, float defaultFloat = default(float))
        {
            if (item == null)
                return defaultFloat;

            float result;
            if (!float.TryParse(item.ToString(), out result))
                return defaultFloat;
            return result;
        }

        /// <summary>
        /// Ases the float for null.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultFloat">The default float.</param>
        /// <returns></returns>
        public static float? AsFloatForNull(this object item, float? defaultFloat = null)
        {
            if (item == null)
                return defaultFloat;

            float result;
            if (!float.TryParse(item.ToString(), out result))
                return defaultFloat;
            return result;
        }

        /// <summary>
        /// Ases the string.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultString">The default string.</param>
        /// <returns></returns>
        public static string AsString(this object item, string defaultString = default(string))
        {
            if (item == null || item.Equals(DBNull.Value))
                return defaultString;

            return item.ToString().Trim();
        }

        public static string AsStringNotTrim(this object item, string defaultString = default(string))
        {
            if (item == null || item.Equals(DBNull.Value))
                return defaultString;

            return item.ToString();
        }
        /// <summary>
        /// Ases the date time.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultDateTime">The default date time.</param>
        /// <returns></returns>
        public static DateTime AsDateTime(this object item, DateTime defaultDateTime = default(DateTime))
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            DateTime result;
            if (!DateTime.TryParse(item.ToString(), out result))
                return defaultDateTime;

            return result;
        }
        public static TimeSpan AsTimSpan(this object item, TimeSpan defaultTimeSpan = default(TimeSpan))
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultTimeSpan;

            TimeSpan result;
            if (!TimeSpan.TryParse(item.ToString(), out result))
                return defaultTimeSpan;

            return result;
        }
        public static TimeSpan? AsTimSpanForNull(this object item, TimeSpan? defaultTimeSpan = null)
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultTimeSpan;

            TimeSpan result;
            if (!TimeSpan.TryParse(item.ToString(), out result))
                return defaultTimeSpan;

            return result;
        }
        public static DateTimeOffset AsDateTimeOffset(this object item, DateTimeOffset defaultDateTime = default(DateTimeOffset))
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            DateTimeOffset result;
            if (!DateTimeOffset.TryParse(item.ToString(), out result))
                return defaultDateTime;

            return result;
        }


        /// <summary>
        /// Ases the date time for null.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultDateTime">The default date time.</param>
        /// <returns></returns>
        public static DateTime? AsDateTimeForNull(this object item, DateTime? defaultDateTime = null)
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultDateTime;

            DateTime result;
            if (!DateTime.TryParse(item.ToString(), out result))
                return defaultDateTime;

            return result;
        }

        /// <summary>
        /// Ases the bool.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultBool">if set to <c>true</c> [default bool].</param>
        /// <returns></returns>
        public static bool AsBool(this object item, bool defaultBool = default(bool))
        {
            if (item == null)
                return defaultBool;
            return new List<string> { "yes", "y", "true" }.Contains(item.ToString().ToLower());
        }

        /// <summary>
        /// Ases the bool for null.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultBool">The default bool.</param>
        /// <returns></returns>
        public static bool? AsBoolForNull(this object item, bool? defaultBool = null)
        {
            if (item == null || string.IsNullOrEmpty(item.ToString()))
                return defaultBool;

            return new List<string> { "yes", "y", "true" }.Contains(item.ToString().ToLower());
        }

        public static byte[] AsByteArray(this byte[] bytes)
        {
            return bytes;
        }

        /// <summary>
        /// Transform string into byte array.
        /// </summary>
        /// <param name="s">The object to be transformed.</param>
        /// <returns>The transformed byte array.</returns>
        public static byte[] AsByteArray(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return Convert.FromBase64String(s);
        }

        /// <summary>
        /// Transform object into base64 string.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <returns>The base64 string value.</returns>
        public static string AsBase64String(this object item)
        {
            if (item == null)
                return null;

            return Convert.ToBase64String((byte[])item);
        }

        /// <summary>
        /// Ases the base64 string.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        //public static string AsBase64String(this Binary item)
        //{
        //    if (item == null)
        //        return null;

        //    return Convert.ToBase64String(item.ToArray());
        //}

        /// <summary>
        /// Transform base64 string to Binary data type. 
        /// Note: This is used in LINQ to SQL only.
        /// </summary>
        /// <param name="s">The base 64 string to be transformed.</param>
        /// <returns>The Binary value.</returns>
        //public static Binary AsBinary(this string s)
        //{
        //    if (string.IsNullOrEmpty(s))
        //        return null;

        //    return new Binary(Convert.FromBase64String(s));
        //}

        /// <summary>
        /// Transform object into Guid data type.
        /// </summary>
        /// <param name="item">The object to be transformed.</param>
        /// <returns>The Guid value.</returns>
        public static Guid AsGuid(this object item)
        {
            try { return new Guid(item.ToString()); }
            catch { return Guid.Empty; }
        }

        /// <summary>
        /// Concatenates SQL and ORDER BY clauses into a single string. 
        /// </summary>
        /// <param name="sql">The SQL string</param>
        /// <param name="sortExpression">The Sort Expression.</param>
        /// <returns>Contatenated SQL Statement.</returns>
        public static string OrderBy(this string sql, string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
                return sql;

            return sql + " ORDER BY " + sortExpression;
        }

        /// <summary>
        /// Takes an enumerable source and returns a comma separate string.
        /// Handy to build SQL Statements (for example with IN () statements) from object collections.
        /// </summary>
        /// <typeparam name="T">The Enumerable type</typeparam>
        /// <typeparam name="TU">The original data type (typically identities - int).</typeparam>
        /// <param name="source">The enumerable input collection.</param>
        /// <param name="func">The function that extracts property value in object.</param>
        /// <returns>The comma separated string.</returns>
        public static string CommaSeparate<T, TU>(this IEnumerable<T> source, Func<T, TU> func)
        {
            return string.Join(",", source.Select(s => func(s).ToString()).ToArray());
        }
    }
}