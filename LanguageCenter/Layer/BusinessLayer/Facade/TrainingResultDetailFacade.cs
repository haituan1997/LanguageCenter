using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class TrainingResultDetailFacade
    {
        SqlServerTrainingResultDetail sqlServerTrainingResultDetail = new SqlServerTrainingResultDetail(); 
        public IEnumerable<TrainingResultDetail> Get_TrainingResultDetails(long trainingResultID,int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerTrainingResultDetail.Get_TrainingResultDetails(trainingResultID,page, pageSize,orderBy,searchBy);
        }
        
        public TrainingResultDetail Get_TrainingResultDetailByTrainingResultDetailID(long studenIDl)
        {
            return sqlServerTrainingResultDetail.Get_TrainingResultDetailByTrainingResultDetailID(studenIDl);
        }
        public int Count( string whereClause, long trainingResultID)
        {
            return sqlServerTrainingResultDetail.Count( whereClause, trainingResultID);
        }
        public TrainingResultDetailResponse Insert(TrainingResultDetail TrainingResultDetail)
        {
            var response = new TrainingResultDetailResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                TrainingResultDetail.TrainingResultDetailID = sqlServerTrainingResultDetail.GetId();
                sqlServerTrainingResultDetail.Insert(TrainingResultDetail);

                response.TrainingResultDetailID = TrainingResultDetail.TrainingResultDetailID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public TrainingResultDetailResponse Update(TrainingResultDetail TrainingResultDetail)
        {
            var response = new TrainingResultDetailResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerTrainingResultDetail.Update(TrainingResultDetail);

                response.TrainingResultDetailID = TrainingResultDetail.TrainingResultDetailID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public TrainingResultDetailResponse Delete(List<long> ids)
        {
            var response = new TrainingResultDetailResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    var checkxoaall = true;
                    foreach (var item in ids)
                    {
                         
                            sqlServerTrainingResultDetail.Delete(item);

                       
                       
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
        public class TrainingResultDetailResponse : ResponseBase
        {
            public long TrainingResultDetailID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}