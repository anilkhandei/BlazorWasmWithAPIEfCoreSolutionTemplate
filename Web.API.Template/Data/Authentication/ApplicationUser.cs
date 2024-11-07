using Microsoft.AspNetCore.Identity;

namespace Web.API.Template.Data.Authentication
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailId { get; set; }

    }
}
