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
    public class SqlServerStudent
    {
        public IEnumerable<Student> Get_Students(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_student";
            object[] parms = {  "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy};
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make,parms);
        }
        public int Count( string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_Student";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(Student Student )
        {
            const string procedure = "uspInsert_Student";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(Student)).AsString();
        }
        public void Update(Student Student)
        {
            const string procedure = "uspUpdate_Student";
            ForeignLanguageCenterAdapter.Update(procedure, Take(Student)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_Student";
            object[] parms = { "@StudentID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, Student> Make = reader =>
           new Student
           {
               StudentID = reader["StudentID"].AsLong(),
               AvatarUrl = reader["AvatarUrl"].AsString(),
               FirtName = reader["FirtName"].AsString(),
               LastName = reader["LastName"].AsString(),
               Email = reader["Email"].AsString(),
               PhoneNumber = reader["PhoneNumber"].AsString(),
               Description = reader["Description"].AsString(),

           };
        private static object[] Take(Student Student)
        {
            return new object[]
            {
                "@StudentID",Student.StudentID,
                "@AvatarUrl",Student.AvatarUrl,
                "@FirtName",Student.FirtName,
                "@LastName",Student.LastName,
                "@Email",Student.Email,
                "@PhoneNumber",Student.PhoneNumber,
                "@Description",Student.Description,
            };
        }
    }
}