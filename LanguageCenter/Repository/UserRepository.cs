using LanguageCenter.DataLayer.Object;
using LanguageCenter.Layer.BusinessLayer.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class UserRepository
    {
        UserFacade userFacade = new UserFacade();
        public UserRepository()
        {
            userFacade = new UserFacade();
        }

        public User Get_Users(string userName, string pass)
        {
            try 
            {
                return userFacade.Get_Users(userName, pass);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}