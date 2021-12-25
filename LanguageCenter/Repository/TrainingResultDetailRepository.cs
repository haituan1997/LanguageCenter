using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class TrainingResultDetailRepository
    {
        TrainingResultDetailFacade TrainingResultDetailFacade = new TrainingResultDetailFacade();
        public TrainingResultDetailRepository()
        {
            TrainingResultDetailFacade = new TrainingResultDetailFacade();
        }

        public IEnumerable<TrainingResultDetail> Get_TrainingResultDetails(out int total, long trainingResultID, int page, int pageSize, string orderBy = null, string searchBy = null)
        {
            try
            {
                total = TrainingResultDetailFacade.Count(searchBy, trainingResultID);
                return TrainingResultDetailFacade.Get_TrainingResultDetails(trainingResultID, page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<TrainingResultDetail> GetTrainingResultDetailForExport(long classID)
        {
            try
            {
                return TrainingResultDetailFacade.GetTrainingResultDetailForExport(classID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<TrainingResultDetail> Get_TrainingResultDetai_By_StudentID(long studentID)
        {
            try
            {

                return TrainingResultDetailFacade.Get_TrainingResultDetai_By_StudentID(studentID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TrainingResultDetail Get_TrainingResultDetailByTrainingResultDetailID(long TrainingResultDetailID)
        {
            try
            {
                return TrainingResultDetailFacade.Get_TrainingResultDetailByTrainingResultDetailID(TrainingResultDetailID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Insert(TrainingResultDetail TrainingResultDetail)
        {
            try
            {
                var response = TrainingResultDetailFacade.Insert(TrainingResultDetail);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TrainingResultDetailID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string Update(TrainingResultDetail TrainingResultDetail)
        {
            try
            {
                var response = TrainingResultDetailFacade.Update(TrainingResultDetail);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TrainingResultDetailID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string Import(TrainingResultDetail TrainingResultDetail)
        {
            try
            {
                var response = TrainingResultDetailFacade.Import(TrainingResultDetail);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TrainingResultDetailID.ToString();

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
                var response = TrainingResultDetailFacade.Delete(id);
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