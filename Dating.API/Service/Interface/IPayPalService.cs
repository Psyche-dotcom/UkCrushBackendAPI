using Model.DTO.paypal;

namespace Dating.API.Service.Interface
{
    public interface IPayPalService
    {
        Task<AuthResponse> Authenticate();
        Task<CreateOrderResponse> CreateOrder(string value, string currency, string reference);
        Task<CaptureOrderResponse> CaptureOrder(string orderId);
    }
}
