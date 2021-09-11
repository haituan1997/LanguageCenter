using LanguageCenter.DataLayer;
using LanguageCenter.DataLayer.Shared;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.SqlServer
{
    public class SqlServerFileHistoryImport
    {
        public void Insert(FileHistoryImport fileHistoryImport)
        {
            const string procedure = "UspInsert_FileHistoryImport";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(fileHistoryImport)).AsString();
        }

        public IEnumerable<FileHistoryImport> GetByContronllerAndUserId(string controllerName, long userId, byte typeUser)
        {
            const string procedure = "UspGet_FileHistoryImport_By_ControllerName_And_UserID";
            object[] parms = { "@ControllerName", controllerName, "@UserID", userId, "@TypeUser", typeUser };
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make, parms);
        }

        private static readonly Func<IDataReader, FileHistoryImport> Make = reader =>
           new FileHistoryImport
           {
               FileHistoryImportID = reader["FileHistoryImportID"].AsLong(),
               ControllerName = reader["ControllerName"].AsString(),
               UserID = reader["UserID"].AsLong(),
               TypeUser = reader["TypeUser"].AsByte(),
               FileName = reader["FileName"].AsString(),

           };
        private static object[] Take(FileHistoryImport fileHistoryImport)
        {
            return new object[]
            {
                "@FileHistoryImportID",fileHistoryImport.FileHistoryImportID,
                "@ControllerName",fileHistoryImport.ControllerName,
                "@UserID",fileHistoryImport.UserID,
                "@TypeUser",fileHistoryImport.TypeUser,
                "@FileName",fileHistoryImport.FileName ,
            };
        }
    }
}