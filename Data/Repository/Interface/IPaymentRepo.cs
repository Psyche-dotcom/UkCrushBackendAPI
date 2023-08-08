using Model.DTO;
using Model.Enitities;
using PayPalCheckoutSdk.Orders;

namespace Data.Repository.Interface
{
    public interface IPaymentRepo
    {
        Task<Payments> GetPaymentById(string OrderReferenceId);
        Task<bool> AddPayments(Payments payments);
        Task<bool> UpdatePayments(Payments payments);
        Task<IEnumerable<PaymentWithUserInfo>> RetrieveAllPaymentAsync();
        Task<IEnumerable<PaymentWithUserInfo>> RetrieveUserAllPaymentAsync(string userid);
    }
}