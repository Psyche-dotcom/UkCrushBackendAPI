using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Enitities
{
    public class Payments
    {
        [Key]
        public string Id { get; set; }

        public string Amount { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public string PaymentType { get; set; }
        public DateTime PaymentTime { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}