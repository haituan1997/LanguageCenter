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
    public class SqlServerClassStudent
    {
        public IEnumerable<ClassStudent> Get_ClassStudentByClassID(long? classID,int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_StudentByClassID";
            object[] parms = { "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy ,"@ClassID",classID};
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakePaged, parms);
        }
        public IEnumerable<ClassStudent> Get_StudentNotInClass(long classID)
        {
            const string procedure = "uspGet_StudentNotInClass";
            object[] parms = { "@ClassID", classID};
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakStudent, parms);
        }
        public int Count(long? classID,string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_StudentByClassID";
            object[] parms = { "@WhereClause", whereClause,"@ClassID", classID };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(ClassStudent classStudent)
        {
            const string procedure = "uspInsert_ClassStudent";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(classStudent)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_ClassStudent";
            object[] parms = { "@ClassStudentID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, ClassStudent> Make = reader =>
           new ClassStudent
           {
               ClassStudentID = reader["ClassStudentID"].AsLong(),
               ClassID = reader["ClassID"].AsLong(),
               StudentID = reader["StudentID"].AsLong(),

           };
        private static readonly Func<IDataReader, ClassStudent> MakePaged = reader =>
           new ClassStudent
           {
               ClassStudentID = reader["ClassStudentID"].AsLong(),
               ClassID = reader["ClassID"].AsLong(),
               StudentID = reader["StudentID"].AsLong(),
               FirtName = reader["FirtName"].AsString(),
               LastName = reader["LastName"].AsString(),
               FullName = reader["FirtName"].AsString() +" "+ reader["LastName"].AsString(),
               DateOfBirth = reader["DateOfBirth"].AsDateTime(),
               CurrentAddress = reader["CurrentAddress"].AsString(),
               CityName = reader["CityName"].AsString(),
               Email = reader["Email"].AsString(),
               PhoneNumber = reader["PhoneNumber"].AsString(),
           };
        private static readonly Func<IDataReader, ClassStudent> MakStudent = reader =>
           new ClassStudent
           {
               StudentID = reader["StudentID"].AsLong(),
               FirtName = reader["FirtName"].AsString(),
               LastName = reader["LastName"].AsString(),
               DateOfBirth = reader["DateOfBirth"].AsDateTime(),
               CurrentAddress = reader["CurrentAddress"].AsString(),
               CityName = reader["CityName"].AsString(),
               Email = reader["Email"].AsString(),
               PhoneNumber = reader["PhoneNumber"].AsString(),
           };
        private static object[] Take(ClassStudent classStudent)
        {
            return new object[]
            {
                "@ClassStudentID",classStudent.ClassStudentID,
                "@ClassID",classStudent.ClassID,
                "@StudentID",classStudent.StudentID,
            };
        }
    }
}