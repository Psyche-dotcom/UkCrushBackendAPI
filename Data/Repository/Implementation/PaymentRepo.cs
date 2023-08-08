using Data.Context;
using Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Model.DTO;
using Model.Enitities;

namespace Data.Repository.Implementation
{
    public class PaymentRepo : IPaymentRepo
    {
        private DatingSiteContext _context;

        public PaymentRepo(DatingSiteContext context)
        {
            _context = context;
        }

        public async Task<Payments> GetPaymentById(string OrderReferenceId)
        {
            return await _context.Payments.FirstOrDefaultAsync(x => x.OrderReferenceId == OrderReferenceId);
        }
        public async Task<IEnumerable<PaymentWithUserInfo>> RetrieveAllPaymentAsync()
        {
            var payment = await _context.Payments.Include(u => u.User).Select(p => new PaymentWithUserInfo
            {
                Id = p.Id,
                Amount = p.Amount,
                OrderReferenceId = p.OrderReferenceId,
                Description = p.Description,
                PaymentType = p.PaymentType,
                CreatedPaymentTime = p.CreatedPaymentTime,
                CompletePaymentTime = p.CompletePaymentTime,
                IsActive = p.IsActive,
                UserId = p.UserId,
                PaymentStatus = p.PaymentStatus,
                UserName = p.User.UserName,
                FirstName = p.User.FirstName,
                Email = p.User.Email
            }).ToListAsync();
            return payment;
        }
        public async Task<IEnumerable<PaymentWithUserInfo>> RetrieveUserAllPaymentAsync(string userid)
        {
            var paymentsWithUserInfo = await _context.Payments
                .Include(p => p.User)
                .Where(p => p.UserId == userid)
                .Select(p => new PaymentWithUserInfo
                {
                    Id = p.Id,
                    Amount = p.Amount,
                    OrderReferenceId = p.OrderReferenceId,
                    Description = p.Description,
                    UserId= p.UserId,
                    PaymentType = p.PaymentType,
                    CreatedPaymentTime = p.CreatedPaymentTime,
                    CompletePaymentTime = p.CompletePaymentTime,
                    IsActive = p.IsActive,
                    PaymentStatus = p.PaymentStatus,
                    UserName = p.User.UserName,
                    FirstName = p.User.FirstName,
                    Email = p.User.Email
                })
                .ToListAsync();

            return paymentsWithUserInfo;
        }


        public async Task<bool> AddPayments(Payments payments)
        {
            await _context.Payments.AddAsync(payments);
            if ( await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatePayments(Payments payments)
        {
             _context.Payments.Update(payments);
            if ( await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}