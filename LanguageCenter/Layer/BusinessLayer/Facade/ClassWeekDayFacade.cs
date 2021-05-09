using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class ClassWeekDayFacade
    {
        SqlServerClassWeekDay sqlServerClassWeekDay= new SqlServerClassWeekDay();
        public IEnumerable<ClassWeekDay> Get_ClassWeekDayByClassID(long? classID, int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerClassWeekDay.Get_ClassWeekDayByClassID(classID, page, pageSize, orderBy, searchBy);
        }
        public IEnumerable<ClassWeekDay> Get_ClassWeekDayForHome()
        {
            return sqlServerClassWeekDay.Get_ClassWeekDayForHome();
        }
        public ClassWeekDay Get_ClassWeekDayByID(long id)
        {
            return sqlServerClassWeekDay.Get_ClassWeekDayByID(id);
        }
        public int Count(string whereClause, long? classID)
        {
            return sqlServerClassWeekDay.Count(classID, whereClause);
        }
        public classWeekDayResponse Insert(ClassWeekDay classWeekDay)
        {
            var response = new classWeekDayResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerClassWeekDay.Insert(classWeekDay);

                response.classWeekDayID = classWeekDay.ClassWeekDayID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public classWeekDayResponse Update(ClassWeekDay classWeekDay)
        {
            var response = new classWeekDayResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerClassWeekDay.Update(classWeekDay);

                response.classWeekDayID = classWeekDay.ClassWeekDayID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public classWeekDayResponse Delete(List<long> ids)
        {
            var response = new classWeekDayResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var item in ids)
                    {
                        sqlServerClassWeekDay.Delete(item);
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
        public class classWeekDayResponse : ResponseBase
        {
            public long classWeekDayID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}