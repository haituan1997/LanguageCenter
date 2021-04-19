using LanguageCenter.DataLayer.Object;
using LanguageCenter.DataLayer.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LanguageCenter.DataLayer.SqlServer
{
    public class SqlServerClass
    {

        private const string SequenceMajorSpecialMangerDetail = "[dbo].[Seq_MajorSpecialMangerDetail_MajorSpecialMangerDetailID]";
        /// <summary>
        /// Gets the training class identifier.
        /// </summary>
        /// <returns>System.Int64.</returns>
        

        public  IEnumerable< Class> Get_Classes()
        {
            const string procedure = "uspGet_Classes";
            return ForeignLanguageCenterAdapter.ReadList(procedure,Make);
        }

        public int Get_Count()
        {
            const string procedure = "uspGet_Count";
            return ForeignLanguageCenterAdapter.GetCount(procedure);
        }

        //public int Count(DataTable majorSpecial, long? MajorSpecialMangerID = null, string whereClause = null)
        //{
        //    const string procedure = "uspCount_MajorSpecialMangerDetail";
        //    object[] parms = {"@majorSpecial", majorSpecial };
        //    return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        //}



        //public IEnumerable<Class> Paged(int pageIndex, int pageSize, string whereClause, string orderBy, DataTable majorSpecial, long? MajorSpecialMangerID = null)
        //{
        //    const string procedure = "uspGetPaged_MajorSpecialMangerDetail";
        //    object[] parms = {
        //        "@PageIndex", pageIndex,
        //        "@PageSize", pageSize,
        //        "@WhereClause", whereClause,
        //        "@OrderBy", orderBy,
        //        "@majorSpecial",majorSpecial,
        //        "@MajorSpecialMangerID", MajorSpecialMangerID,
        //    };
        //    return ForeignLanguageCenterAdapter.ReadList(procedure, Make, parms);
        //}



        private static readonly Func<IDataReader, Class> Make = reader =>
           new Class
           {
               ClassID = reader["ClassID"].AsLong(),
               ClassName = reader["ClassName"].AsString(),
               StartDate = reader["StartDate"].AsDateTime(),
               EndDate = reader["EndDate"].AsDateTime(),
               Price = reader["Price"].AsDecimal(),
               TeacherID = reader["TeacherID"].AsLong(),
               CourseID = reader["CourseID"].AsLong(),

           };
    }
}