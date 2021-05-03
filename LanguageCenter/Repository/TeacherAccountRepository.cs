using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class TeacherAccountRepository
    {
        TeacherAccountFacade teacherAccountFacade= new TeacherAccountFacade();
        public TeacherAccountRepository()
        {
            teacherAccountFacade = new TeacherAccountFacade();
        }

        public IEnumerable<TeacherAccount> Get_TeacherAccounts(out int total, int page, int pageSize, string orderBy = null, string searchBy = null)
        {
            try
            {
                total = teacherAccountFacade.Count(searchBy);
                return teacherAccountFacade.Get_TeacherAccounts(page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public TeacherAccount Get_StudentAccountByStudentAccountID(long id)
        {
            try
            {
                return teacherAccountFacade.Get_StudentAccountByStudentAccountID(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Insert(TeacherAccount teacherAccount)
        {
            try
            {
                var response = teacherAccountFacade.Insert(teacherAccount);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TeacherAccounID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string Update(TeacherAccount teacherAccount)
        {
            try
            { 
                var response = teacherAccountFacade.Update(teacherAccount);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TeacherAccounID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public void Delete(List<long> id)
        {
            try
            {
                var response = teacherAccountFacade.Delete(id);
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
    }
}