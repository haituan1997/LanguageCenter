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
    public class SqlServerTrainingResultDetail
    {
        private const string SequenceUser = "[dbo].[Seq_TrainingResultDetail_TrainingResultDetailID]";
        public long GetId()
        {
            return (long)ForeignLanguageCenterAdapter.GetSequence(SequenceUser);
        }
        public IEnumerable<TrainingResultDetail> Get_TrainingResultDetails(long trainingResultID, int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_TrainingResultDetail";
            object[] parms = { "@TrainingResultID", trainingResultID, "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy };
            return ForeignLanguageCenterAdapter.ReadDataTableToListByEntity<TrainingResultDetail>(procedure, parms);
        }

        public IEnumerable<TrainingResultDetail> Get_TrainingResultDetai_By_StudentID(long studentID)
        {
            const string procedure = "uspGet_TrainingResultDetai_By_StudentID";
            object[] parms = { "@StudentID", studentID };
            return ForeignLanguageCenterAdapter.ReadDataTableToListByEntity<TrainingResultDetail>(procedure, parms);
        }

        public TrainingResultDetail Get_TrainingResultDetailByTrainingResultDetailID(long TrainingResultDetailID)
        {
            const string procedure = "uspGet_TrainingResultDetailByTrainingResultDetailID";
            object[] parms = { "@TrainingResultDetailID", TrainingResultDetailID };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public int Count(string whereClause = null, long? trainingResultID = null)
        {
            const string procedure = "uspCount_TrainingResultDetail";
            object[] parms = { "@WhereClause", whereClause, "@TrainingResultID", trainingResultID };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(TrainingResultDetail TrainingResultDetail)
        {
            const string procedure = "uspInsert_TrainingResultDetail";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(TrainingResultDetail)).AsString();
        }
        public void Update(TrainingResultDetail TrainingResultDetail)
        {
            const string procedure = "uspUpdate_TrainingResultDetail";
            ForeignLanguageCenterAdapter.Update(procedure, Take(TrainingResultDetail)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_TrainingResultDetail";
            object[] parms = { "@TrainingResultDetailID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, TrainingResultDetail> Make = reader =>
           new TrainingResultDetail
           {
               TrainingResultID = reader["TrainingResultID"].AsLong(),
               TrainingResultDetailID = reader["TrainingResultDetailID"].AsLong(),
               StudentID = reader["StudentID"].AsLong(),
               ScoreFirt = reader["ScoreFirt"].AsDecimal(),
               ScoreLast = reader["ScoreLast"].AsDecimal(),
               LastName = reader["LastName"].AsString(),
               FirtName = reader["FirtName"].AsString(),

           };

        private static object[] Take(TrainingResultDetail TrainingResultDetail)
        {
            return new object[]
            {
                "@TrainingResultID",TrainingResultDetail.TrainingResultID,
                "@TrainingResultDetailID",TrainingResultDetail.TrainingResultDetailID,
                "@StudentID",TrainingResultDetail.StudentID,
                "@ScoreFirt",TrainingResultDetail.ScoreFirt,
                "@ScoreLast",TrainingResultDetail.ScoreLast,
                "@ScorePracticeFirst",TrainingResultDetail.ScorePracticeFirst,
                "@ScorePracticeLast",TrainingResultDetail.ScorePracticeLast,
            };
        }
    }
}