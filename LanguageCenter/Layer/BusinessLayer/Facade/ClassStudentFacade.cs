using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class ClassStudentFacade
    {
        SqlServerClassStudent sqlServerClassStudent = new SqlServerClassStudent();
        public IEnumerable<ClassStudent> Get_ClassStudentByClassID(long? classID, int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerClassStudent.Get_ClassStudentByClassID(classID, page, pageSize, orderBy, searchBy);
        }
        public IEnumerable<ClassStudent> Get_StudentNotInClass(long classID)
        {
            return sqlServerClassStudent.Get_StudentNotInClass(classID);
        }
        public IEnumerable<ClassStudent> GetData_For_Export(long classId)
        {
            return sqlServerClassStudent.GetData_For_Export(classId);
        }
        public IEnumerable<ClassStudent> Get_StudentInClass(long classID, string notInStudentIds = null)
        {
            return sqlServerClassStudent.Get_StudentInClass(classID, notInStudentIds);
        }
        public IEnumerable<ClassStudent> Get_StudentInClassNotInTrainingResult(long trainingResultID, long classID, long? TrainingResult = null)
        {
            return sqlServerClassStudent.Get_StudentInClassNotInTrainingResult(trainingResultID, classID, TrainingResult);
        }

        public int Count(string whereClause, long? classID)
        {
            return sqlServerClassStudent.Count(classID, whereClause);
        }
        public ClassStudentResponse Insert(ClassStudent classStudent)
        {
            var response = new ClassStudentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerClassStudent.Insert(classStudent);

                response.ClassStudentID = classStudent.ClassStudentID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public ClassStudentResponse Delete(List<long> ids)
        {
            var response = new ClassStudentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var item in ids)
                    {
                        sqlServerClassStudent.Delete(item);
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
        public class ClassStudentResponse : ResponseBase
        {
            public long ClassStudentID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}