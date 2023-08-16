using Microsoft.AspNetCore.Identity;
using Model.Enum;

namespace Model.Enitities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string ProfilePicture { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Location { get; set; }
        public int TimeAvailable { get; set; }
        public bool IsUserOnline { get; set; } = false;
        public bool UserIsTaken { get; set; } = false;
        public bool SuspendUser { get; set; } = false;
        public ICollection<CallRecord> CallRecords { get; set; }
        public ICollection<Timers> Timers { get; set; }
        public ICollection<Payments> Payments { get; set; }
    }
}