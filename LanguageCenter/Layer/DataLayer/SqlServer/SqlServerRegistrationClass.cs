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
    public class SqlServerRegistrationClass
    {
        public void Insert(RegistrationClass registrationClass)
        {
            const string procedure = "uspInsert_RegistrationClass";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(registrationClass)).AsString();
        }
        public void Delete(RegistrationClass registrationClass)
        {
            const string procedure = "uspDelete_RegistrationClass_By_StudentID_And_ClassID";
            object[] parms = { "@StudentID", registrationClass.StudentID, "@ClassID", registrationClass.ClassID};
            ForeignLanguageCenterAdapter.Delete(procedure,parms).AsString();
        }

        public IEnumerable<RegistrationClass> GetByStudentId(long studentId)
        {
            const string procedure = "uspGet_RegistrationClass_By_StudentID";
            object[] parms = { "@StudentID", studentId };
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make, parms);
        }

        public Tuple<List<ClassStudent>, object[]> GetPage_RegistrationClassByClassID(long? classID, int page, int pageSize, string orderBy, string searchBy)
        {
            const string procedure = "uspGetPage_RegistrationClassByClassID";
            object[] parms =
            {
                "ClassID", classID,
                "@Page", page,
                "@PageSize", pageSize,
                "@OrderByColumn", orderBy,
                "@SearchBy", searchBy
            };

            object[] outputParams =
            {
                 "@TotalRecords", DbType.Int32
            };

            return ForeignLanguageCenterAdapter.ReadListWithOutputParam(procedure, MakePaged, parms, outputParams);
        }
        private static object[] Take(RegistrationClass registrationClass)
        {
            return new object[]
            {
                "@RegistrationClassID",registrationClass.RegistrationClassID,
                "@StudentID",registrationClass.StudentID,
                "@ClassID",registrationClass.ClassID,
            };
        }
        private static readonly Func<IDataReader, RegistrationClass> Make = reader =>
          new RegistrationClass
          {
              RegistrationClassID = reader["RegistrationClassID"].AsLong(),
              StudentID = reader["StudentID"].AsLong(),
              ClassID = reader["ClassID"].AsLong(),
          };
        private static readonly Func<IDataReader, ClassStudent> MakePaged = reader =>
          new ClassStudent
          {
              ClassID = reader["ClassID"].AsLong(),
              StudentID = reader["StudentID"].AsLong(),
              FirtName = reader["FirtName"].AsString(),
              LastName = reader["LastName"].AsString(),
              FullName = reader["FirtName"].AsString() + " " + reader["LastName"].AsString(),
              DateOfBirth = reader["DateOfBirth"].AsDateTime(),
              CurrentAddress = reader["CurrentAddress"].AsString(),
              Email = reader["Email"].AsString(),
              PhoneNumber = reader["PhoneNumber"].AsString(),
          };
    }
}