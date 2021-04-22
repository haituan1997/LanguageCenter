using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class UserFacade
    {
        DataLayer.SqlServer.SqlServerUser sqlServerUser = new DataLayer.SqlServer.SqlServerUser();
        public class UserResponse : ResponseBase
        {
            public long UserID { get; set; }
            public string ResponseMessage { get; set; }
        }

      

        public User Get_Users(string userName, string pass)
        {
            return sqlServerUser.Get_Users(userName, pass);
        }
    }
}