using Microsoft.AspNetCore.Identity;

namespace Finance.Data
{
    public class FinanceUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int LoginAttempts { get; set; }
    }
}
