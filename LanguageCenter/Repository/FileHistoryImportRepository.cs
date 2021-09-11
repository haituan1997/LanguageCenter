using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class FileHistoryImportRepository
    {
        FileHistoryImportFacade fileHistoryImportFacade = new FileHistoryImportFacade();
        public IEnumerable<FileHistoryImport> GetByContronllerAndUserId(string controllerName, long userId, byte typeUser)
        {
            try
            {
                return fileHistoryImportFacade.GetByContronllerAndUserId(controllerName, userId, typeUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Insert(FileHistoryImport fileHistoryImport)
        {
            try
            {
                var response = fileHistoryImportFacade.Insert(fileHistoryImport);
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