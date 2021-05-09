using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.DataLayer.Object;
using LanguageCenter.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class ClassFacade
    {
        SqlServerClass sqlServerClass = new SqlServerClass();
        private void BeforeInsert(ref Class objClass)
        {
            objClass.ClassID = sqlServerClass.GetClassID();
        }
        public IEnumerable<Class> Get_Classes(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerClass.Get_Classes(page, pageSize, orderBy, searchBy);
        }
        public IEnumerable<Class> Get_AllClasses()
        {
            return sqlServerClass.Get_AllClasses();
        }
        public IEnumerable<Class> Get_AllClassesByIndex(int? index)
        {
            return sqlServerClass.Get_AllClassesByIndex( index);
        }
        
        public IEnumerable<Class> Get_Class_ByCourseID(long id, int indexNumber)
        {
            return sqlServerClass.Get_Class_ByCourseID(  id,   indexNumber);
        }
        public Class Get_ClassByClassID(long id)
        {
            return sqlServerClass.Get_ClassByClassID(id);
        }
        public int Count(string whereClause)
        {
            return sqlServerClass.Count(whereClause);
        }
        public ClassResponse Insert(Class objClass)
        {
            var response = new ClassResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                var obj = sqlServerClass.Get_ClassByClassName(objClass.ClassName);
                if (obj != null)
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = "Tên lơp đã tồn tại trong hệ thống";
                    return response;
                }
                BeforeInsert(ref objClass);
                sqlServerClass.Insert(objClass);

                response.ClassID = objClass.ClassID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public ClassResponse Update(Class objClass)
        {
            var response = new ClassResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                var obj = sqlServerClass.Get_ClassByClassName(objClass.ClassName);
                if (obj != null && obj.ClassID != objClass.ClassID)
                {
                    response.Acknowledge = AcknowledgeType.Failure;
                    response.Message = "Tên lơp đã tồn tại trong hệ thống";
                    return response;
                }
                sqlServerClass.Update(objClass);

                response.ClassID = objClass.ClassID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public ClassResponse Delete(List<long> ids)
        {
            var response = new ClassResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var item in ids)
                    {
                        sqlServerClass.Delete(item);
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
        public class ClassResponse : ResponseBase
        {
            public long ClassID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}