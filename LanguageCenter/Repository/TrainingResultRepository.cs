using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class TrainingResultRepository
    {
        TrainingResultFacade TrainingResultFacade= new TrainingResultFacade();
        public TrainingResultRepository()
        {
            TrainingResultFacade = new TrainingResultFacade();
        }

        public IEnumerable<TrainingResult> Get_TrainingResults(out int total,int page , int pageSize , string orderBy = null, string searchBy = null)
        {
            try
            {
                total = TrainingResultFacade.Count(searchBy);
                return TrainingResultFacade.Get_TrainingResults( page, pageSize, orderBy, searchBy); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        public TrainingResult Get_TrainingResultByTrainingResultID(long TrainingResultID)
        {
            try
            { 
                return TrainingResultFacade.Get_TrainingResultByTrainingResultID(TrainingResultID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public long Insert(TrainingResult TrainingResult)
        {
            try
            {
                var response = TrainingResultFacade.Insert(TrainingResult);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TrainingResultID;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public string Update(TrainingResult TrainingResult)
        {
            try
            {
                var response = TrainingResultFacade.Update(TrainingResult);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TrainingResultID.ToString();

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
                var response = TrainingResultFacade.Delete(id);
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