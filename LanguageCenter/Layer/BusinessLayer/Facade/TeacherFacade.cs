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