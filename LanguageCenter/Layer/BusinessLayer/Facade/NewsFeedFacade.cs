using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class NewsFeedFacade
    {
        SqlServerNewsFeed sqlServerNewsFeed = new SqlServerNewsFeed(); 
        public IEnumerable<NewsFeed> Get_NewsFeeds(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerNewsFeed.Get_NewsFeeds(page,pageSize,orderBy,searchBy);
        }
        
        public NewsFeed Get_NewsFeedByNewFeedID(long studenIDl)
        {
            return sqlServerNewsFeed.Get_NewsFeedByNewsFeedID(studenIDl);
        }
        public int Count( string whereClause)
        {
            return sqlServerNewsFeed.Count( whereClause);
        }
        public NewsFeedResponse Insert(NewsFeed NewsFeed)
        {
            var response = new NewsFeedResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                NewsFeed.NewFeedID = sqlServerNewsFeed.GetId();
                sqlServerNewsFeed.Insert(NewsFeed);

                response.NewFeedID = NewsFeed.NewFeedID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public NewsFeedResponse Update(NewsFeed NewsFeed)
        {
            var response = new NewsFeedResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerNewsFeed.Update(NewsFeed);

                response.NewFeedID = NewsFeed.NewFeedID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public NewsFeedResponse Delete(List<long> ids)
        {
            var response = new NewsFeedResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    var checkxoaall = true;
                    foreach (var item in ids)
                    {
                         
                            sqlServerNewsFeed.Delete(item);

                       
                       
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
        public class NewsFeedResponse : ResponseBase
        {
            public long NewFeedID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}