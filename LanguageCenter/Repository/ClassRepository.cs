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

        public IEnumerable<Class> Get_Classes()
        {
            try 
            {
                return classFacade.Get_Classes();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}