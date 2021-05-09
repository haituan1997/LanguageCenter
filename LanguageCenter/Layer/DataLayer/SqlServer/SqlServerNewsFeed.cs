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
    public class SqlServerNewsFeed
    {
        private const string SequenceUser = "[dbo].[Seq_NewsFeed_NewsFeedID]";
        public long GetId()
        {
            return (long)ForeignLanguageCenterAdapter.GetSequence(SequenceUser);
        }
        public IEnumerable<NewsFeed> Get_NewsFeeds(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_NewsFeed";
            object[] parms = {  "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy};
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make,parms);
        }
       
        
        public NewsFeed Get_NewsFeedByNewsFeedID(long NewsFeedID)
        {
            const string procedure = "uspGet_NewsFeedByNewsFeedID";
            object[] parms = { "@NewsFeedID", NewsFeedID };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public int Count( string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_NewsFeed";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(NewsFeed NewsFeed )
        {
            const string procedure = "uspInsert_NewsFeed";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(NewsFeed)).AsString();
        }
        public void Update(NewsFeed NewsFeed)
        {
            const string procedure = "uspUpdate_NewsFeed";
            ForeignLanguageCenterAdapter.Update(procedure, Take(NewsFeed)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_NewsFeed";
            object[] parms = { "@NewsFeedID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, NewsFeed> Make = reader =>
           new NewsFeed
           {
               NewFeedID = reader["NewFeedID"].AsLong(),
               Title = reader["Title"].AsString(),
               Code = reader["Code"].AsString(),
               Description = reader["Description"].AsString(),
               Createdate = reader["Createdate"].AsDateTime(),


           };
        
        private static object[] Take(NewsFeed NewsFeed)
        {
            return new object[]
            {
                "@NewFeedID",NewsFeed.NewFeedID,
                "@Title",NewsFeed.Title,
                "@Code",NewsFeed.Code,
                "@Description",NewsFeed.Description,
                "@Createdate",NewsFeed.Createdate,
            };
        }
    }
}