using Data.Repository.Interface;
using Dating.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;

namespace Dating.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayments _pay;
        private readonly IPaymentRepo _paydb;

        public PaymentsController(IPayments pay, IPaymentRepo paydb)
        {
            _pay = pay;
            _paydb = paydb;
        }

        [HttpGet("get-OrderId")]
        public async Task<IActionResult> GetOrder(string paymentType, string currency, string description)
        {
            Amount amount = paymentType == "Type1" ? Amount.Type1 : paymentType == "Type2" ? Amount.Type2 : paymentType == "Type3" ? Amount.Type3 : 0.00;

            var data = await _pay.MakeOrder((decimal)amount, currency, description);
            if (data == null)
            {
                return Problem("Order not set");
            }
            else
                return Ok(data.Result.Links.ToDictionary(links => links.Rel, links => links.Href));
        }

        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment(string OrderId)
        {
            var data = await _pay.ConfirmPayment(OrderId);
            return Ok(data);
        }

        [HttpGet("check-active-payment")]
        public async Task<IActionResult> CheckActivePayment(string OrderId)
        {
            var data = await _paydb.IsPaymentActive(OrderId);
            return Ok(data);
        }

        [HttpPut("deactivate-payment")]
        public async Task<IActionResult> DeactivatePayment(string OrderId)
        {
            var data = _paydb.DeactivatePayment(OrderId);
            return Ok(data);
        }

        [HttpGet("get-payment-by-orderId")]
        public async Task<IActionResult> GetPaymentByOrderId(string OrderId)
        {
            var data = _paydb.GetPaymentById(OrderId);
            return Ok(data);
        }
    }
}