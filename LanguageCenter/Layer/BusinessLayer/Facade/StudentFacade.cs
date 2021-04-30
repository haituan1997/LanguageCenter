using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class StudentFacade
    {
        SqlServerStudent sqlServerStudent = new SqlServerStudent();
        public IEnumerable<Student> Get_Students(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerStudent.Get_Students(page,pageSize,orderBy,searchBy);
        }
        public int Count( string whereClause)
        {
            return sqlServerStudent.Count( whereClause);
        }
        public StudentResponse Insert(Student Student)
        {
            var response = new StudentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerStudent.Insert(Student);

                response.StudentID = Student.StudentID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public class StudentResponse : ResponseBase
        {
            public long StudentID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}