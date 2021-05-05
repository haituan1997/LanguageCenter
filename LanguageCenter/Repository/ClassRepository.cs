using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.Layer.BusinessLayer.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class ClassRepository
    {
        ClassFacade classFacade = new ClassFacade();
        public ClassRepository()
        {
            classFacade = new ClassFacade();
        }

        public IEnumerable<Class> Get_Classes(out int total, int page, int pageSize, string orderBy = null, string searchBy = null)
        {
            try
            {
                total = classFacade.Count(searchBy);
                return classFacade.Get_Classes(page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<Class> GetAll_Classes()
        {
            try
            {
                return classFacade.GetAll_Classes();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Class Get_ClassByClassID(long id)
        {
            try
            {
                return classFacade.Get_ClassByClassID(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public long Insert(Class objClass)
        {
            try
            {
                var response = classFacade.Insert(objClass);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return objClass.ClassID;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string Update(Class objClass)
        {
            try
            {
                var response = classFacade.Update(objClass);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.ClassID.ToString();

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
                var response = classFacade.Delete(id);
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