using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class ClassStudentRepository
    {
        ClassStudentFacade classStudentFacade= new ClassStudentFacade();
        public ClassStudentRepository()
        {
            classStudentFacade = new ClassStudentFacade();
        }

        public IEnumerable<ClassStudent> Get_ClassStudentByClassID(long? classID,out int total, int page, int pageSize, string orderBy = null, string searchBy = null)
        {
            try
            {
                total = classStudentFacade.Count(searchBy, classID);
                return classStudentFacade.Get_ClassStudentByClassID(classID,page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<ClassStudent> Get_StudentNotInClass(long classID)
        {
            try
            {
                return classStudentFacade.Get_StudentNotInClass(classID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<ClassStudent> Get_StudentInClass(long classID)
        {
            try
            {
                return classStudentFacade.Get_StudentInClass(classID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<ClassStudent> Get_StudentInClassNotInTrainingResult(long  trainingResltid,long classID, long? TrainingResult = null)
        {
            try
            {
                return classStudentFacade.Get_StudentInClassNotInTrainingResult(trainingResltid,classID, TrainingResult);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public long Insert(ClassStudent classStudent)
        {
            try
            {
                var response = classStudentFacade.Insert(classStudent);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return classStudent.ClassStudentID;

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
                var response = classStudentFacade.Delete(id);
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