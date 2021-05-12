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
    public class SqlServerTeacher
    {
        public IEnumerable<Teacher> Get_Teacheres(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_Teacher";
            object[] parms = { "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy };
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make, parms);
        }
        public int Count(string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_Teacher";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public IEnumerable<Teacher> Get_AllTeacheres()
        {
            const string procedure = "uspGet_Tearcheres";
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make);
        }
        public IEnumerable<Teacher> Get_TeacheresNotInTeacherAccount()
        {
            const string procedure = "uspGet_TeacheresNotInTeacherAccount";
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make);
        }
        public void Insert(Teacher teacher )
        {
            const string procedure = "uspInsert_Teacher";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(teacher)).AsString();
        }
        public void Update(Teacher teacher)
        {
            const string procedure = "uspUpdate_Teacher";
            ForeignLanguageCenterAdapter.Update(procedure, Take(teacher)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_Teacher";
            object[] parms = { "@TeacherID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, Teacher> Make = reader =>
           new Teacher
           {
               TeacherID = reader["TeacherID"].AsLong(),
               AvatarUrl = reader["AvatarUrl"].AsString(),
               FirtName = reader["FirtName"].AsString(),
               LastName = reader["LastName"].AsString(),
               Email = reader["Email"].AsString(),
               NumberPhone = reader["NumberPhone"].AsString(),
               Description = reader["Description"].AsString(),

           };
        private static object[] Take(Teacher teacher)
        {
            return new object[]
            {
                "@TeacherID",teacher.TeacherID,
                "@AvatarUrl",teacher.AvatarUrl,
                "@FirtName",teacher.FirtName,
                "@LastName",teacher.LastName,
                "@Email",teacher.Email,
                "@NumberPhone",teacher.NumberPhone,
                "@Description",teacher.Description,
            };
        }
    }
}