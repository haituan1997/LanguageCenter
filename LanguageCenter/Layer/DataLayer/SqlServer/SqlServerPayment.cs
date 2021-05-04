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
    public class SqlServerPayment
    {
        private const string SequenceUser = "[dbo].[Seq_Payment_PaymentID]";
        public long GetId()
        {
            return (long)ForeignLanguageCenterAdapter.GetSequence(SequenceUser);
        }
        public IEnumerable<Payment> Get_Payments(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_Payment";
            object[] parms = {  "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy};
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakePage, parms);
        }
        public Payment Get_PaymentByPaymentID(long PaymentID)
        {
            const string procedure = "uspGet_PaymentByPaymentID";
            object[] parms = { "@PaymentID", PaymentID };
            return ForeignLanguageCenterAdapter.Read(procedure, Make, parms);
        }
        public int Count( string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_Payment";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public void Insert(Payment Payment )
        {
            const string procedure = "uspInsert_Payment";
            ForeignLanguageCenterAdapter.Insert(procedure, Take(Payment)).AsString();
        }
        public void Update(Payment Payment)
        {
            const string procedure = "uspUpdate_Payment";
            ForeignLanguageCenterAdapter.Update(procedure, Take(Payment)).AsString();
        }
        public void Delete(long id)
        {
            const string procedure = "uspDelete_Payment";
            object[] parms = { "@PaymentID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        private static readonly Func<IDataReader, Payment> Make = reader =>
           new Payment
           {
               PaymentID = reader["PaymentID"].AsLong(),
               FirtName = reader["FirtName"].AsString(),
               LastName = reader["LastName"].AsString(),
               PaymentDate = reader["PaymentDate"].AsDateTime(),
               Amount = reader["Amount"].AsDecimal(),
               PaymentMethodID = reader["PaymentMethodID"].AsShort(),
               StudentID = reader["StudentID"].AsLong(),
               ClassID = reader["ClassID"].AsLong(),
               Status = reader["Status"].AsShort(),
               ClassName = reader["ClassName"].AsString(), 
           };
        private static readonly Func<IDataReader, Payment> MakePage = reader =>
           new Payment
           {
               PaymentID = reader["PaymentID"].AsLong(),
               FirtName = reader["FirtName"].AsString(),
               LastName = reader["LastName"].AsString(),
               PaymentDate = reader["PaymentDate"].AsDateTime(),
               Amount = reader["Amount"].AsDecimal(),
               PaymentMethodID = reader["PaymentMethodID"].AsShort(),
               StudentID = reader["StudentID"].AsLong(),
               ClassID = reader["ClassID"].AsLong(),
               Status = reader["Status"].AsShort(),
               ClassName = reader["ClassName"].AsString(),
               FullName = reader["FirtName"].AsString() + reader["LastName"].AsString(),
           };
        private static object[] Take(Payment Payment)
        {
            return new object[]
            {
                "@PaymentID",Payment.PaymentID,
                "@PaymentDate",Payment.PaymentDate,
                "@Amount",Payment.Amount,
                "@PaymentMethodID",Payment.PaymentMethodID,
                "@StudentID",Payment.StudentID,
                "@ClassID",Payment.ClassID,
                "@Status",Payment.Status,
            };
        }
    }
}