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
    public class SqlServerTeacherAccount
    {
        public IEnumerable<TeacherAccount> uspGet_TeacherAccounts(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_TeacherAccount";
            object[] parms = { "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy };
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakePaged, parms);
        }
        public TeacherAccount uspGet_TeacherAccountByTeacherAccountID(long id)
        {
            const string procedure = "uspGet_TeacherAccountByTeacherAccountID";
            object[] parms = { "@TeacherAccountID", id };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public TeacherAccount uspGet_TeacheresByUserLogin(string userLogin)
        {
            const string procedure = "uspGet_TeacheresByUserLogin";
            object[] parms = { "@UserLogin", userLogin };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public int Count(string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_TeacherAccount";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(TeacherAccount teacherAccount)
        {
            const string procedure = "uspInsert_TeacherAccount";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(teacherAccount)).AsString();
        }
        public void Update(TeacherAccount teacherAccount)
        {
            const string procedure = "uspUpdate_TeacherAccount";
            ForeignLanguageCenterAdapter.Update(procedure, Take(teacherAccount)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_TeacherAccount";
            object[] parms = { "@TeacherAccountID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, TeacherAccount> Make = reader =>
        new TeacherAccount
        {
            TeacherAccountID = reader["TeacherAccountID"].AsLong(),
            IsActive = reader["IsActive"].AsBool(),
            UserLogin = reader["UserLogin"].AsString(),
            PassWordLogin = reader["PassWordLogin"].AsString(),
            TeacherID = reader["TeacherID"].AsLong(),

        };
        private static readonly Func<IDataReader, TeacherAccount> MakePaged = reader =>
        new TeacherAccount
        {
            TeacherAccountID = reader["TeacherAccountID"].AsLong(),
            IsActive = reader["IsActive"].AsBool(),
            UserLogin = reader["UserLogin"].AsString(),
            PassWordLogin = reader["PassWordLogin"].AsString(),
            TeacherID = reader["TeacherID"].AsLong(),
            FirtName = reader["FirtName"].AsString(),
            LastName = reader["LastName"].AsString(),
            FullName = reader["FirtName"].AsString()+ " " + reader["LastName"].AsString(),
            Description = reader["Description"].AsString(),

        };

        private static object[] Take(TeacherAccount teacherAccount)
        {
            return new object[]
            {

        "@TeacherAccountID",teacherAccount.TeacherAccountID,
        "@IsActive",teacherAccount.IsActive,
        "@UserLogin",teacherAccount.UserLogin,
        "@PassWordLogin",teacherAccount.PassWordLogin,
        "@TeacherID",teacherAccount.TeacherID,
            };
        }
    }
}