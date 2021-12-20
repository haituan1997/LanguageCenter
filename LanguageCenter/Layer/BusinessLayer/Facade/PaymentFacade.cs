using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class PaymentFacade
    {
        SqlServerPayment sqlServerPayment = new SqlServerPayment();
        public IEnumerable<Payment> Get_Payments(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerPayment.Get_Payments(page, pageSize, orderBy, searchBy);
        }
        public Tuple<List<Payment>, object[]> GetPaged_FilterPrice(FilterPrice filterPrice)
        {
            return sqlServerPayment.GetPaged_FilterPrice(filterPrice);
        }
        public IEnumerable<Payment> GetStudentPaidByClassID(long classID)
        {
            return sqlServerPayment.GetStudentPaidByClassID(classID);
        }

        public Payment Get_PaymentByPaymentID(long studenIDl)
        {
            return sqlServerPayment.Get_PaymentByPaymentID(studenIDl);
        }
        public int Count(string whereClause)
        {
            return sqlServerPayment.Count(whereClause);
        }
        public PaymentResponse Insert(Payment Payment)
        {
            var response = new PaymentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                Payment.PaymentID = sqlServerPayment.GetId();
                sqlServerPayment.Insert(Payment);

                response.PaymentID = Payment.PaymentID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public PaymentResponse Update(Payment Payment)
        {
            var response = new PaymentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerPayment.Update(Payment);

                response.PaymentID = Payment.PaymentID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public PaymentResponse Delete(List<long> ids)
        {
            var response = new PaymentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    var checkxoaall = true;
                    foreach (var item in ids)
                    {

                        sqlServerPayment.Delete(item);

                    }
                    if (checkxoaall == false)
                    {
                        throw new Exception("Sinh viên này đã được dùng ở chức năng khác.");
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
        public class PaymentResponse : ResponseBase
        {
            public long PaymentID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}