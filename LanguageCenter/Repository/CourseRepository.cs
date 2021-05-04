using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class CourseRepository
    {
        CourseFacade courseFacade= new CourseFacade();
        public CourseRepository()
        {
            courseFacade = new CourseFacade();
        }

        public IEnumerable<Course> Get_Courses(out int total, int page, int pageSize, string orderBy = null, string searchBy = null)
        {
            try
            {
                total = courseFacade.Count(searchBy);
                return courseFacade.Get_Courses(page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<Course> Get_AllCourses()
        {
            try
            {
                return courseFacade.Get_AllCourses();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Course Get_CourseByCourseID(long id)
        {
            try
            {
                return courseFacade.Get_CourseByCourseID(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Insert(Course course)
        {
            try
            {
                var response = courseFacade.Insert(course);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.CourseID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string Update(Course course)
        {
            try
            {
                var response = courseFacade.Update(course);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.CourseID.ToString();

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
                var response = courseFacade.Delete(id);
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



        public IEnumerable<Category> Get_Categories()
        {
            try
            {
                return courseFacade.Get_Categories();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string InsertCategory(Category category)
        {
            try
            {
                var response = courseFacade.InsertCategory(category);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.CategoryID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public IEnumerable<Language> Get_Languages()
        {
            try
            {
                return courseFacade.Get_Languages();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string InsertLanguage(Language language)
        {
            try
            {
                var response = courseFacade.InsertLanguage(language);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.LanguageID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public IEnumerable<Level> Get_Levels()
        {
            try
            {
                return courseFacade.Get_Levels();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string InsertLevel(Level level)
        {
            try
            {
                var response = courseFacade.InsertLevel(level);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.LevelID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}