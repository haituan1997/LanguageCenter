using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class NewsFeedRepository
    {
        NewsFeedFacade NewsFeedFacade= new NewsFeedFacade();
        public NewsFeedRepository()
        {
            NewsFeedFacade = new NewsFeedFacade();
        }

        public IEnumerable<NewsFeed> Get_NewsFeeds(out int total,int page , int pageSize , string orderBy = null, string searchBy = null)
        {
            try
            {
                total = NewsFeedFacade.Count(searchBy);
                return NewsFeedFacade.Get_NewsFeeds( page, pageSize, orderBy, searchBy); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        public NewsFeed Get_NewsFeedByNewFeedID(long NewFeedID)
        {
            try
            { 
                return NewsFeedFacade.Get_NewsFeedByNewFeedID(NewFeedID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Insert(NewsFeed NewsFeed)
        {
            try
            {
                var response = NewsFeedFacade.Insert(NewsFeed);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.NewFeedID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public string Update(NewsFeed NewsFeed)
        {
            try
            {
                var response = NewsFeedFacade.Update(NewsFeed);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.NewFeedID.ToString();

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
                var response = NewsFeedFacade.Delete(id);
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