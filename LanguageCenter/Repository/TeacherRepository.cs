using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class TeacherRepository
    {
        TeacherFacade teacherFacade= new TeacherFacade();
        public TeacherRepository()
        {
            teacherFacade = new TeacherFacade();
        }
        public IEnumerable<Teacher> Get_PagedTeacheres(out int total, int page, int pageSize, string orderBy = null, string searchBy = null)
        {
            try
            {
                total = teacherFacade.Count(searchBy);
                return teacherFacade.Get_Teacheres(page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Teacher> Get_AllTeacheres()
        {
            try
            {
                return teacherFacade.Get_AllTeacheres();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<Teacher> Get_TeacheresNotInTeacherAccount()
        {
            try
            {
                return teacherFacade.Get_TeacheresNotInTeacherAccount();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string InsertOrUpdate(Teacher teacher)
        {
            try
            {
                var response = teacherFacade.InsertOrUpdate(teacher);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TeacherID.ToString();

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
                var response = teacherFacade.Delete(id);
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