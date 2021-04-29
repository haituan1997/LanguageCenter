using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class TeacherFacade
    {
        SqlServerTeacher sqlServerTeacher = new SqlServerTeacher();
        public IEnumerable<Teacher> Get_Teacheres()
        {
            return sqlServerTeacher.Get_Teacheres();
        }
        //public class ClassResponse : ResponseBase
        //{
        //    public long ClassID { get; set; }
        //    public string ResponseMessage { get; set; }
        //}
    }
}