using LanguageCenter.DataLayer.Object;
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
    public class SqlServerStudentAccount
    {
       
        public IEnumerable<StudentAccount> Get_StudentAccounts(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_StudentAccount";
            object[] parms = {  "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy};
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakePage, parms);
        }
        public StudentAccount Get_StudentAccountByStudentAccountID(long StudentAccountID)
        {
            const string procedure = "uspGet_StudentAccountByStudentAccountID";
            object[] parms = { "@StudentAccountID", StudentAccountID };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public StudentAccount Get_StudentAccountByStudentID(long StudentID)
        {
            const string procedure = "uspGet_StudentAccountByStudentID";
            object[] parms = { "@StudentID", StudentID };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public User Get_StudentAccountByUserName(string userName)
        {
            const string procedure = "uspGet_StudentAccountByStudentUserLogin";
            object[] parms = { "@UserLogin", userName };
            return ForeignLanguageCenterAdapter.Read(procedure, MakeRegister, parms);
        }
        
        public int Count( string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_StudentAccount";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(StudentAccount StudentAccount )
        {
            const string procedure = "uspInsert_StudentAccount";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(StudentAccount)).AsString();
        }
        public void Update(StudentAccount StudentAccount)
        {
            const string procedure = "uspUpdate_StudentAccount";
            ForeignLanguageCenterAdapter.Update(procedure, Take(StudentAccount)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_StudentAcount";
            object[] parms = { "@StudentAccountID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, StudentAccount> Make = reader =>
           new StudentAccount
           {
               StudentAccountID = reader["StudentAccountID"].AsLong(),
               StudentID = reader["StudentID"].AsLong(),
               UserLogin = reader["UserLogin"].AsString(),
               PassWordLogin = reader["PassWordLogin"].AsString(),
               IsActive = reader["IsActive"].AsBool(),

           };
        private static readonly Func<IDataReader, User> MakeRegister = reader =>
         new User
         {
             UserID = reader["UserID"].AsLong(),
             Username = reader["Username"].AsString(),
             Password = reader["Password"].AsString(),
             FullName = reader["FullName"].AsString(),
             TypeUser = reader["TypeUser"].AsShort(),

         };
        private static readonly Func<IDataReader, StudentAccount> MakePage = reader =>
           new StudentAccount
           {
               StudentAccountID = reader["StudentAccountID"].AsLong(),
               StudentID = reader["StudentID"].AsLong(),
               UserLogin = reader["UserLogin"].AsString(),
               LastName = reader["LastName"].AsString(), 
               FirtName = reader["FirtName"].AsString(),
               FullName = reader["LastName"].AsString()+ " "+reader["FirtName"].AsString(),
               CurrentAddress = reader["CurrentAddress"].AsString(),
               DateOfBirth = reader["DateOfBirth"].AsDateTimeForNull(),
               PassWordLogin = reader["PassWordLogin"].AsString(),
               IsActive = reader["IsActive"].AsBool(),

           };

        private static object[] Take(StudentAccount StudentAccount)
        {
            return new object[]
            {
                "@StudentAccountID",StudentAccount.StudentAccountID,
                "@StudentID",StudentAccount.StudentID,
                "@PassWordLogin",StudentAccount.PassWordLogin,
                "@UserLogin",StudentAccount.UserLogin,
                "@IsActive",StudentAccount.IsActive,
            };
        }
    }
}