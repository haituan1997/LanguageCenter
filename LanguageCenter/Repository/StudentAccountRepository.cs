using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class StudentAccountRepository
    {
        StudentAccountFacade StudentAccountFacade= new StudentAccountFacade();
        public StudentAccountRepository()
        {
            StudentAccountFacade = new StudentAccountFacade();
        }

        public IEnumerable<StudentAccount> Get_StudentAccounts(out int total,int page , int pageSize , string orderBy = null, string searchBy = null)
        {
            try
            {
                total = StudentAccountFacade.Count(searchBy);
                return StudentAccountFacade.Get_StudentAccounts( page, pageSize, orderBy, searchBy); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public StudentAccount Get_StudentAccountByStudentAccountID(long StudentAccountID)
        {
            try
            { 
                return StudentAccountFacade.Get_StudentAccountByStudentAccountID(StudentAccountID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public StudentAccount Get_StudentAccountByStudentID(long studentID)
        {
            try
            {
                return StudentAccountFacade.Get_StudentAccountByStudentID(studentID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public User Get_StudentAccountByUserName(string user)
        {
            try
            {
                return StudentAccountFacade.Get_StudentAccountByUserName(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Insert(StudentAccount StudentAccount)
        {
            try
            {
                var response = StudentAccountFacade.Insert(StudentAccount);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.StudentAccountID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public string Update(StudentAccount StudentAccount)
        {
            try
            {
                var response = StudentAccountFacade.Update(StudentAccount);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.StudentAccountID.ToString();

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
                var response = StudentAccountFacade.Delete(id);
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