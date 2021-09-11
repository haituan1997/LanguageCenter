using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class FileHistoryImportFacade
    {
        SqlServerFileHistoryImport sqlServerFileHistoryImport = new SqlServerFileHistoryImport();
        public IEnumerable<FileHistoryImport> GetByContronllerAndUserId(string controllerName, long userId, byte typeUser)
        {
            return sqlServerFileHistoryImport.GetByContronllerAndUserId(controllerName, userId, typeUser);
        }
        public FileHistoryImportResponse Insert(FileHistoryImport fileHistoryImport)
        {
            var response = new FileHistoryImportResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerFileHistoryImport.Insert(fileHistoryImport);
                response.FileHistoryImportID = fileHistoryImport.FileHistoryImportID;
                return response;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
        }

        public class FileHistoryImportResponse : ResponseBase
        {
            public long FileHistoryImportID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}