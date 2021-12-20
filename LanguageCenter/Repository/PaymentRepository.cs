using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class PaymentRepository
    {
        PaymentFacade PaymentFacade = new PaymentFacade();
        public PaymentRepository()
        {
            PaymentFacade = new PaymentFacade();
        }

        public IEnumerable<Payment> Get_Payments(out int total, int page, int pageSize, string orderBy = null, string searchBy = null)
        {
            try
            {
                total = PaymentFacade.Count(searchBy);
                return PaymentFacade.Get_Payments(page, pageSize, orderBy, searchBy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Tuple<List<Payment>, object[]> GetPaged_FilterPrice(FilterPrice filterPrice)
        {
            try
            {
                return PaymentFacade.GetPaged_FilterPrice(filterPrice);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public IEnumerable<Payment> GetStudentPaidByClassID(long classID)
        {
            try
            {
                return PaymentFacade.GetStudentPaidByClassID(classID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Payment Get_PaymentByPaymentID(long PaymentID)
        {
            try
            {
                return PaymentFacade.Get_PaymentByPaymentID(PaymentID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Insert(Payment Payment)
        {
            try
            {
                var response = PaymentFacade.Insert(Payment);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.PaymentID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public string Update(Payment Payment)
        {
            try
            {
                var response = PaymentFacade.Update(Payment);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.PaymentID.ToString();

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
                var response = PaymentFacade.Delete(id);
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