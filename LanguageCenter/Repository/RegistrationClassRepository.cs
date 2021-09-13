using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class RegistrationClassRepository
    {
        RegistrationClassFacade registrationClassFacade = new RegistrationClassFacade();
        public IEnumerable<RegistrationClass> GetByStudentId(long studentId)
        {
            try
            {
                return registrationClassFacade.GetByStudentId(studentId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Insert(RegistrationClass registrationClass)
        {
            try
            {
                var response = registrationClassFacade.Insert(registrationClass);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public void Delete(RegistrationClass registrationClass)
        {
            try
            {
                var response = registrationClassFacade.Delete(registrationClass);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public Tuple<List<ClassStudent>, object[]> GetPage_RegistrationClassByClassID(long? classID, int page, int pageSize, string orderBy, string searchBy)
        {
            try
            {
                return registrationClassFacade.GetPage_RegistrationClassByClassID(classID, page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}