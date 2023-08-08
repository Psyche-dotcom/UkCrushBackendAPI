using Model.DTO;

namespace Dating.API.Service.Interface
{
    public interface IPaymentService
    {
        Task<ResponseDto<Dictionary<string, string>>> MakeOrder(string paymentType, string userid);
        Task<ResponseDto<string>> ConfirmPayment(string token);
        Task<ResponseDto<IEnumerable<PaymentWithUserInfo>>> RetrieveUserAllPaymentAsync(string userid);
        Task<ResponseDto<IEnumerable<PaymentWithUserInfo>>> RetrieveAllPaymentAsync();
    }
}