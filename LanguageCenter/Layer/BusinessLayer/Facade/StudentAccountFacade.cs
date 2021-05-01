using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class StudentAccountFacade
    {
        SqlServerStudentAccount sqlServerStudentAccount = new SqlServerStudentAccount();
        public IEnumerable<StudentAccount> Get_StudentAccounts(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerStudentAccount.Get_StudentAccounts(page,pageSize,orderBy,searchBy);
        }
        public StudentAccount Get_StudentAccountByStudentAccountID(long studenIDl)
        {
            return sqlServerStudentAccount.Get_StudentAccountByStudentAccountID(studenIDl);
        }
        public int Count( string whereClause)
        {
            return sqlServerStudentAccount.Count( whereClause);
        }
        public StudentAccountResponse Insert(StudentAccount StudentAccount)
        {
            var response = new StudentAccountResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerStudentAccount.Insert(StudentAccount);

                response.StudentAccountID = StudentAccount.StudentAccountID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public StudentAccountResponse Update(StudentAccount StudentAccount)
        {
            var response = new StudentAccountResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerStudentAccount.Update(StudentAccount);

                response.StudentAccountID = StudentAccount.StudentAccountID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public StudentAccountResponse Delete(List<long> ids)
        {
            var response = new StudentAccountResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var item in ids)
                    {
                        sqlServerStudentAccount.Delete(item);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public class StudentAccountResponse : ResponseBase
        {
            public long StudentAccountID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}