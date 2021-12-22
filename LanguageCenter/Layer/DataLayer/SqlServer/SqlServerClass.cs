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
        private const string SequenceClassID = "Seq_Class_ClassID";
        public long GetClassID()
        {
            return (long)ForeignLanguageCenterAdapter.GetSequence(SequenceClassID);
        }
        public IEnumerable<Class> Get_Classes(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_Class";
            object[] parms = { "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy };
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakePaged, parms);
        }
        public IEnumerable<Class> Get_AllClasses()
        {
            const string procedure = "uspGet_AllData";
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make);
        }
        public IEnumerable<Class> Get_AllClassesByIndex(int? index)
        {
            const string procedure = "uspGet_ClassByIndex";
            object[] parms = { "@IndexNumber", index };
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakeClassByCourseID, parms);
        }
        public IEnumerable<Class> Get_Class_ByCourseID(long id, int indexNumber)
        {
            const string procedure = "uspGet_ClassByCourseID";
            object[] parms = { "@CourseID", id, "@IndexNumber", indexNumber };
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakeClassByCourseID, parms);
        }
        public IEnumerable<Class> Get_AllClassesNotTrainingResult()
        {
            const string procedure = "Get_AllClassesNotTrainingResult";
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make);
        }


        public Class Get_ClassByClassID(long id)
        {
            const string procedure = "uspGet_ClassByClassID";
            object[] parms = { "@ClassID", id };
            return ForeignLanguageCenterAdapter.ReadDataAndMapToObject<Class>(procedure, parms);
        }
        public Class Get_ClassByClassName(string className)
        {
            const string procedure = "uspGet_ClassByClassName";
            object[] parms = { "@ClassName", className };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public int Count(string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_Class";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(Class objclass)
        {
            const string procedure = "uspInsert_Class";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(objclass)).AsString();
        }
        public void Update(Class objclass)
        {
            const string procedure = "uspUpdate_Class";
            ForeignLanguageCenterAdapter.Update(procedure, Take(objclass)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_Class";
            object[] parms = { "@ClassID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, Class> Make = reader =>
           new Class
           {
               ClassID = reader["ClassID"].AsLong(),
               ClassName = reader["ClassName"].AsString(),
               StartDate = reader["StartDate"].AsDateTimeForNull(),
               EndDate = reader["EndDate"].AsDateTimeForNull(),
               Price = reader["Price"].AsDecimal(),
               TeacherID = reader["TeacherID"].AsLong(),
               CourseID = reader["CourseID"].AsLong(),
               IsCreated = reader["IsCreated"].AsBool(),
           };
        private static readonly Func<IDataReader, Class> MakeClassByCourseID = reader =>
           new Class
           {
               ClassID = reader["ClassID"].AsLong(),
               ClassName = reader["ClassName"].AsString(),
               StartDate = reader["StartDate"].AsDateTimeForNull(),
               EndDate = reader["EndDate"].AsDateTimeForNull(),
               Price = reader["Price"].AsDecimal(),
               TeacherID = reader["TeacherID"].AsLong(),
               CourseID = reader["CourseID"].AsLong(),
               IsCreated = reader["IsCreated"].AsBool(),
               NumberClass = reader["NumberClass"].AsInt(),
               FirtName = reader["FirtName"].AsString(),
               LastName = reader["LastName"].AsString(),
               AvatarUrl = reader["AvatarUrl"].AsString(),
           };
        private static readonly Func<IDataReader, Class> MakePaged = reader =>
            new Class
            {
                ClassID = reader["ClassID"].AsLong(),
                ClassName = reader["ClassName"].AsString(),
                StartDate = reader["StartDate"].AsDateTime(),
                EndDate = reader["EndDate"].AsDateTime(),
                Price = reader["Price"].AsDecimal(),
                TeacherID = reader["TeacherID"].AsLong(),
                CourseID = reader["CourseID"].AsLong(),
                FirtName = reader["FirtName"].AsString(),
                LastName = reader["LastName"].AsString(),
                FullName = reader["FirtName"].AsString() + " " + reader["LastName"].AsString(),
                CourseName = reader["Name"].AsString(),
            };
        private static object[] Take(Class objclass)
        {
            return new object[]
            {
                "@ClassID",objclass.ClassID,
                "@ClassName",objclass.ClassName,
                "@StartDate",objclass.StartDate,
                "@EndDate",objclass.EndDate,
                "@Price",objclass.Price,
                "@TeacherID",objclass.TeacherID,
                "@CourseID",objclass.CourseID,
                "@IsCreated",objclass.IsCreated,
                "@IsComplete",objclass.IsComplete,
            };
        }


    }
}