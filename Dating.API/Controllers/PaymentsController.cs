using Data.Repository.Interface;
using Dating.API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dating.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _pay;
        private readonly IPaymentRepo _paydb;

        public PaymentsController(IPaymentService pay, IPaymentRepo paydb)
        {
            _pay = pay;
            _paydb = paydb;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("create/buy_minute")]
        public async Task<IActionResult> GetOrder(string paymentType, string user_id)
        {

            var result = await _pay.MakeOrder(paymentType, user_id);

            if (result.StatusCode == 200 || result.StatusCode == 201)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }


        }

        [HttpGet("webhook/confirm-payment")]
        public async Task<IActionResult> ConfirmPayment(string token)
        {
            var result = await _pay.ConfirmPayment(token);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles ="ADMIN")]
        [HttpGet("get-payment-by-orderId")]
        public async Task<IActionResult> GetPaymentByOrderId(string OrderId)
        {
            var data = await _paydb.GetPaymentById(OrderId);
            return Ok(data);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("user/all/{user_id}")]
        public async Task<IActionResult> UserPaymentHistory(string user_id)
        {
            var result = await _pay.RetrieveUserAllPaymentAsync(user_id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles ="ADMIN")]
        [HttpGet("user/all")]
        public async Task<IActionResult> AllPaymentHistory()
        {
            var result = await _pay.RetrieveAllPaymentAsync();
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}