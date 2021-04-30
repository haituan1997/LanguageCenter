using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class StudentRepository
    {
        StudentFacade studentFacade= new StudentFacade();
        public StudentRepository()
        {
            studentFacade = new StudentFacade();
        }

        public IEnumerable<Student> Get_Students(out int total,int page , int pageSize , string orderBy = null, string searchBy = null)
        {
            try
            {
                total = studentFacade.Count(searchBy);
                return studentFacade.Get_Students( page, pageSize, orderBy, searchBy); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Student Get_StudentByStudentID(long studentID)
        {
            try
            { 
                return studentFacade.Get_StudentByStudentID(studentID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Insert(Student student)
        {
            try
            {
                var response = studentFacade.Insert(student);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.StudentID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public string Update(Student student)
        {
            try
            {
                var response = studentFacade.Update(student);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.StudentID.ToString();

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
                var response = studentFacade.Delete(id);
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