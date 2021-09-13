using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class RegistrationClassFacade
    {
        SqlServerRegistrationClass SqlServerRegistrationClass = new SqlServerRegistrationClass();

        public IEnumerable<RegistrationClass> GetByStudentId(long studentId)
        {
            return SqlServerRegistrationClass.GetByStudentId(studentId);
        }
        public RegistrationClassResponse Insert(RegistrationClass registrationClass)
        {
            var response = new RegistrationClassResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                SqlServerRegistrationClass.Insert(registrationClass);
                response.RegistrationClassID = registrationClass.RegistrationClassID;
                return response;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
        }

        public RegistrationClassResponse Delete(RegistrationClass registrationClass)
        {
            var response = new RegistrationClassResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                SqlServerRegistrationClass.Delete(registrationClass);
                response.RegistrationClassID = registrationClass.RegistrationClassID;
                return response;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
        }

        public Tuple<List<ClassStudent>, object[]> GetPage_RegistrationClassByClassID(long? classID, int page, int pageSize, string orderBy, string searchBy)
        {
            return SqlServerRegistrationClass.GetPage_RegistrationClassByClassID(classID, page, pageSize, orderBy, searchBy);
        }
        public class RegistrationClassResponse : ResponseBase
        {
            public long RegistrationClassID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}