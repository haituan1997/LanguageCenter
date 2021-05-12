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
        public IEnumerable<Teacher> Get_Teacheres(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerTeacher.Get_Teacheres(page, pageSize, orderBy, searchBy);
        }
        public IEnumerable<Teacher> Get_AllTeacheres()
        {
            return sqlServerTeacher.Get_AllTeacheres();
        }
        public IEnumerable<Teacher> Get_TeacheresNotInTeacherAccount()
        {
            return sqlServerTeacher.Get_TeacheresNotInTeacherAccount();
        }
        public int Count(string whereClause)
        {
            return sqlServerTeacher.Count(whereClause);
        }
        public TeacherResponse InsertOrUpdate(Teacher teacher)
        {
            var response = new TeacherResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (teacher != null)
                {
                    if (teacher.IsEdit)
                    {
                        sqlServerTeacher.Update(teacher);
                        response.TeacherID = teacher.TeacherID;
                    }
                    else
                    {
                        sqlServerTeacher.Insert(teacher);
                        response.TeacherID = teacher.TeacherID;
                    }
                }
                else 
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = "Đã xảy ra lỗi";
                    return response;
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

        public TeacherResponse Delete(List<long> ids)
        {
            var response = new TeacherResponse { Acknowledge = AcknowledgeType.Success };
            try { 
                if (ids.Count>0)
                {
                    foreach (var item in ids)
                    {
                        sqlServerTeacher.Delete(item);
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


        public class TeacherResponse : ResponseBase
        {
            public long TeacherID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}