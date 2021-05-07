using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class ClassWeekDayRepository
    {
        ClassWeekDayFacade classWeekDayFacade = new ClassWeekDayFacade();
        public ClassWeekDayRepository()
        {
            classWeekDayFacade = new ClassWeekDayFacade();
        }

        public IEnumerable<ClassWeekDay> Get_ClassWeekDayByClassID(long? classID, out int total, int page, int pageSize, string orderBy = null, string searchBy = null)
        {
            try
            {
                total = classWeekDayFacade.Count(searchBy, classID);
                return classWeekDayFacade.Get_ClassWeekDayByClassID(classID, page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ClassWeekDay Get_ClassWeekDayByID(long id)
        {
            try
            {
                return classWeekDayFacade.Get_ClassWeekDayByID(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public long Insert(ClassWeekDay classWeekDay)
        {
            try
            {
                var response = classWeekDayFacade.Insert(classWeekDay);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return classWeekDay.ClassWeekDayID;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string Updaet(ClassWeekDay classWeekDay)
        {
            try
            {
                var response = classWeekDayFacade.Update(classWeekDay);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return classWeekDay.ClassWeekDayID.ToString();

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
                var response = classWeekDayFacade.Delete(id);
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