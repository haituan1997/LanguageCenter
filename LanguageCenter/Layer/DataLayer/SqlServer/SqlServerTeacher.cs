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
        public IEnumerable<Teacher> Get_Teacheres()
        {
            const string procedure = "uspGet_Tearcheres";
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make);
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
    }
}