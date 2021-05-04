using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class TeacherAccountFacade
    {
        SqlServerTeacherAccount sqlServerTeacherAccount= new SqlServerTeacherAccount();
        public IEnumerable<TeacherAccount> Get_TeacherAccounts(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerTeacherAccount.uspGet_TeacherAccounts(page, pageSize, orderBy, searchBy);
        }
        public TeacherAccount Get_StudentAccountByStudentAccountID(long id)
        {
            return sqlServerTeacherAccount.uspGet_TeacherAccountByTeacherAccountID(id);
        }
        public int Count(string whereClause)
        {
            return sqlServerTeacherAccount.Count(whereClause);
        }
        public TeacherAccountResponse Insert(TeacherAccount teacherAccount)
        {
            var response = new TeacherAccountResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                var obj = sqlServerTeacherAccount.uspGet_TeacheresByUserLogin(teacherAccount.UserLogin);
                if (obj!=null)
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = "Tài khoản đã tồn tại trong hệ thống";
                    return response;
                }
                sqlServerTeacherAccount.Insert(teacherAccount);

                response.TeacherAccounID = teacherAccount.TeacherAccountID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public TeacherAccountResponse Update(TeacherAccount teacherAccount)
        {
            var response = new TeacherAccountResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                var obj = sqlServerTeacherAccount.uspGet_TeacheresByUserLogin(teacherAccount.UserLogin);
                if (obj != null)
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = "Tài khoản đã tồn tại trong hệ thống";
                    return response;
                }
                sqlServerTeacherAccount.Update(teacherAccount);

                response.TeacherAccounID = teacherAccount.TeacherAccountID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public TeacherAccountResponse Delete(List<long> ids)
        {
            var response = new TeacherAccountResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var item in ids)
                    {
                        sqlServerTeacherAccount.Delete(item);
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
        public class TeacherAccountResponse : ResponseBase
        {
            public long TeacherAccounID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}