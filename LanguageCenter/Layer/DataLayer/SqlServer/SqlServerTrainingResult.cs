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
    public class SqlServerTrainingResult
    {
        private const string SequenceUser = "[dbo].[Seq_TrainingResult_TrainingResultID]";
        public long GetId()
        {
            return (long)ForeignLanguageCenterAdapter.GetSequence(SequenceUser);
        }
        public IEnumerable<TrainingResult> Get_TrainingResults(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_TrainingResult";
            object[] parms = {  "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy};
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make,parms);
        }
       
        
        public TrainingResult Get_TrainingResultByTrainingResultID(long TrainingResultID)
        {
            const string procedure = "uspGet_TrainingResultByTrainingResultD";
            object[] parms = { "@TrainingResultID", TrainingResultID };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public int Count( string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_TrainingResult";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(TrainingResult TrainingResult )
        {
            const string procedure = "uspInsert_TrainingResult";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(TrainingResult)).AsString();
        }
        public void Update(TrainingResult TrainingResult)
        {
            const string procedure = "uspUpdate_TrainingResult";
            ForeignLanguageCenterAdapter.Update(procedure, Take(TrainingResult)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_TrainingResult";
            object[] parms = { "@TrainingResultID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, TrainingResult> Make = reader =>
           new TrainingResult
           {
               TrainingResultID = reader["TrainingResultID"].AsLong(),
               ClassID = reader["ClassID"].AsLong(),
               ClassName = reader["ClassName"].AsString(), 

           };
        
        private static object[] Take(TrainingResult TrainingResult)
        {
            return new object[]
            {
                "@TrainingResultID",TrainingResult.TrainingResultID,
                "@ClassID",TrainingResult.ClassID,
                "@IsCreated",TrainingResult.IsCreated, 
            };
        }
    }
}