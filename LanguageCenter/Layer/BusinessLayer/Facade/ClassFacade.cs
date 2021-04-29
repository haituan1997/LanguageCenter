using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class ClassFacade
    {
        SqlServerClass sqlServerClass = new SqlServerClass();
        public class ClassResponse : ResponseBase
        {
            public long ClassID { get; set; }
            public string ResponseMessage { get; set; }
        }

        public ClassResponse Insert_Class(Class cl)
        {
            var response = new ClassResponse { Acknowledge = AcknowledgeType.Success };
            try 
            {
                return response;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
        }

        public IEnumerable<Class> Get_Classes()
        {
            return sqlServerClass.Get_Classes();
        }
    }
}