using Model.DTO;
using PayPalCheckoutSdk.Orders;

namespace Dating.API.Service.Interface
{
    public interface IPayments
    {
        Task<ResponseDto<Order>> MakeOrder(decimal amount, string currency, string description);

        Task<ResponseDto<Order>> ConfirmPayment(string OrderId);
    }
}