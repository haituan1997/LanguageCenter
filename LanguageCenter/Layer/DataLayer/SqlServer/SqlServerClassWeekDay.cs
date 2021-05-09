using LanguageCenter.DataLayer;
using LanguageCenter.DataLayer.Shared;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.SqlServer
{
    public class SqlServerClassWeekDay
    {
        public IEnumerable<ClassWeekDay> Get_ClassWeekDayByClassID(long? classID, int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_ClassWeekDay";
            object[] parms = { "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy, "@ClassID", classID };
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make, parms);
        }
        public IEnumerable<ClassWeekDay> Get_ClassWeekDayForHome()
        {
            const string procedure = "uspGetPaged_ClassWeekDay";
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make);
        }
        public  ClassWeekDay Get_ClassWeekDayByID(long? id)
        {
            const string procedure = "uspGet_ClassWeekDayByID";
            object[] parms = { "@ClassWeekDayID", id };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public int Count(long? classID, string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_ClassWeekDay";
            object[] parms = { "@WhereClause", whereClause, "@ClassID", classID };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(ClassWeekDay classWeekDay)
        {
            const string procedure = "uspInsert_ClassWeekDay";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(classWeekDay)).AsString();
        }
        public void Update(ClassWeekDay classWeekDay)
        {
            const string procedure = "uspUpdate_ClassWeekDay";
            ForeignLanguageCenterAdapter.Update(procedure, Take(classWeekDay)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_ClassWeekDay";
            object[] parms = { "@ClassWeekDayID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, ClassWeekDay> Make = reader =>
           new ClassWeekDay
           {
               ClassWeekDayID = reader["ClassWeekDayID"].AsLong(),
               ClassID = reader["ClassID"].AsLong(),
               ClassWeekDayTime = reader["ClassWeekDayTime"].AsDateTime(),
               StartTime = reader["StartTime"].AsString(),
               EndTime = reader["EndTime"].AsString(),

           };
        private static object[] Take(ClassWeekDay classWeekDay)
        {
            return new object[]
            {
                "@ClassWeekDayID",classWeekDay.ClassWeekDayID,
                "@ClassID",classWeekDay.ClassID,
                "@ClassWeekDayTime",classWeekDay.ClassWeekDayTime,
                "@StartTime",classWeekDay.StartTime,
                "@EndTime",classWeekDay.EndTime,
            };
        }

    }
}