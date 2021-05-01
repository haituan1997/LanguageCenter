using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class CourseFacade
    {
        SqlServerCourse  sqlServerCourse= new SqlServerCourse();
        public IEnumerable<Course> Get_Courses(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerCourse.Get_Courses(page, pageSize, orderBy, searchBy);
        }
        public Course Get_CourseByCourseID(long id)
        {
            return sqlServerCourse.GetCourseByCouresID(id);
        }
        public int Count(string whereClause)
        {
            return sqlServerCourse.Count(whereClause);
        }
        public CourseResponse Insert(Course course)
        {
            var response = new CourseResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerCourse.InsertCourse(course);
                response.CourseID = course.CourseID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public CourseResponse Update(Course course)
        {
            var response = new CourseResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerCourse.UpdateCourse(course);

                response.CourseID = course.CourseID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public CourseResponse Delete(List<long> ids)
        {
            var response = new CourseResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var item in ids)
                    {
                        sqlServerCourse.DeleteCourse(item);
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
        public class CourseResponse : ResponseBase
        {
            public long CourseID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}