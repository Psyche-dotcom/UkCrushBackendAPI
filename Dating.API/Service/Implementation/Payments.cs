using Data.Repository.Interface;
using Dating.API.Service.Interface;
using Model.DTO;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace Dating.API.Service.Implementation
{
    public class Payments : IPayments
    {
        private PayPalHttpClient _client;
        private IPaymentRepo _paymentdb;

        public Payments(PayPalHttpClient client, IPaymentRepo paymentdb)
        {
            _client = client;
            _paymentdb = paymentdb;
        }

        public async Task<ResponseDto<Order>> MakeOrder(decimal amount, string currency, string description)
        {
            var result = new ResponseDto<Order>();
            try
            {
                var orderRequest = OrderRequest(amount.ToString("0.00"), currency, description);
                var response = await _client.Execute(orderRequest);
                var order = response.Result<Order>();

                result.Result = order;
                result.DisplayMessage = $"{order.Id}";
                result.StatusCode = 201;
                result.Succeeded = true;

                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMessages.Add(ex.Message);
                result.DisplayMessage = $"Failed to make Order";
                result.StatusCode = StatusCodes.Status204NoContent;
                result.Succeeded = false;

                return result;
            }
        }

        private OrdersCreateRequest OrderRequest(string amount, string currency, string description)
        {
            var orderRequest = new OrdersCreateRequest()
            {
                Body = new OrderRequest()
                {
                    CheckoutPaymentIntent = "CAPTURE",
                    PurchaseUnits = new List<PurchaseUnitRequest>()
                    {
                        new PurchaseUnitRequest()
                        {
                            AmountWithBreakdown = new AmountWithBreakdown()
                            {
                                CurrencyCode = currency,
                                Value = amount
                            },
                            Description = description
                        }
                    },
                    ApplicationContext = new ApplicationContext()
                    {
                        PaymentMethod = new PaymentMethod
                        {
                            PayeePreferred = "IMMEDIATE_PAYMENT_REQUIRED",
                            PayerSelected = "PAYPAL"
                        },
                        LandingPage = "NO_PREFERENCE",
                        ReturnUrl = "https://localhost:7187/swagger/index.html",
                        CancelUrl = "https://bing.com/"
                    }
                }
            };

            return orderRequest;
        }

        public async Task<ResponseDto<Order>> ConfirmPayment(string OrderId)
        {
            try
            {
                var capture = new OrdersGetRequest(OrderId);

                capture.Body = new Order()
                {
                    Id = OrderId
                };

                var response = await _client.Execute(capture);
                var order = response.Result<Order>();
                bool IsComplete = order.Status == "APPROVED";
                if (IsComplete && _paymentdb.GetPaymentById(OrderId) == null)
                {
                    var res = _paymentdb.AddPayments(order);
                }

                return new ResponseDto<Order>()
                {
                    Succeeded = IsComplete,
                    Result = order,
                    StatusCode = IsComplete ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<Order>()
                {
                    ErrorMessages = new List<string>() { ex.Message }
                };
            }
        }
    }
}