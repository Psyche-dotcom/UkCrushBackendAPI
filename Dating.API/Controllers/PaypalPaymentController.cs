using Dating.API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Enum;

namespace Dating.API.Controllers
{
    
    [Route("api/[controller]")]
    
    [ApiController]
    public class PaypalPaymentController : ControllerBase
    {
        private readonly IPayPalService _payPalService;

        public PaypalPaymentController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("order")]
        public async Task<IActionResult> Order(AmountToPay amountToPay)
        {
            try
            {
               
                var price = ((int)amountToPay).ToString();
                var currency = "USD";

             
                var reference = Guid.NewGuid().ToString();

                var response = await _payPalService.CreateOrder(price, currency, reference);

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Capture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _payPalService.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

               

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }
    }
}
