using Data.Context;
using Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Model.Enitities;
using PayPalCheckoutSdk.Orders;

namespace Data.Repository.Implementation
{
    public class PaymentRepo : IPaymentRepo
    {
        private DatingSiteContext _context;

        public PaymentRepo(DatingSiteContext context)
        {
            _context = context;
        }

        public async Task<Payments> GetPaymentById(string OrderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(x => x.ReferenceNumber == OrderId);
        }

        public Payments AddPayments(Order order)
        {
            try
            {
                var data = new Payments()
                {
                    Id = Guid.NewGuid().ToString(),
                    ReferenceNumber = order.Id,
                    Amount = order.PurchaseUnits.First(x => x.AmountWithBreakdown != null).AmountWithBreakdown.Value,
                    //Description = "Payment",
                    IsActive = true,
                    PaymentTime = DateTime.Now,
                    PaymentType = "payment-type",
                    UserId = Guid.NewGuid().ToString() + "null user",
                };

                _context.Payments.Add(data);
                return _context.SaveChanges() > 0 ? data : null;
            }
            catch (Exception ex)
            {
                return new Payments()
                {
                    Description = ex.Message,
                };
            }
        }

        public async Task<bool> IsPaymentActive(string orderId)
        {
            return _context.Payments.FirstOrDefault(x => x.ReferenceNumber == orderId).IsActive;
        }

        public bool DeactivatePayment(string orderId)
        {
            var data = _context.Payments.FirstOrDefault(x => x.ReferenceNumber == orderId);
            data.IsActive = false;
            _context.Payments.Update(data);
            return _context.SaveChanges() > 0;
        }
    }
}