using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class TrainingResultFacade
    {
        SqlServerTrainingResult sqlServerTrainingResult = new SqlServerTrainingResult(); 
        public IEnumerable<TrainingResult> Get_TrainingResults(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerTrainingResult.Get_TrainingResults(page,pageSize,orderBy,searchBy);
        }
        
        public TrainingResult Get_TrainingResultByTrainingResultID(long studenIDl)
        {
            return sqlServerTrainingResult.Get_TrainingResultByTrainingResultID(studenIDl);
        }
        public int Count( string whereClause)
        {
            return sqlServerTrainingResult.Count( whereClause);
        }
        public TrainingResultResponse Insert(TrainingResult TrainingResult)
        {
            var response = new TrainingResultResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                TrainingResult.TrainingResultID = sqlServerTrainingResult.GetId();
                sqlServerTrainingResult.Insert(TrainingResult);

                response.TrainingResultID = TrainingResult.TrainingResultID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public TrainingResultResponse Update(TrainingResult TrainingResult)
        {
            var response = new TrainingResultResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerTrainingResult.Update(TrainingResult);

                response.TrainingResultID = TrainingResult.TrainingResultID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public TrainingResultResponse Delete(List<long> ids)
        {
            var response = new TrainingResultResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    var checkxoaall = true;
                    foreach (var item in ids)
                    {
                         
                            sqlServerTrainingResult.Delete(item);

                       
                       
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
        public class TrainingResultResponse : ResponseBase
        {
            public long TrainingResultID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}