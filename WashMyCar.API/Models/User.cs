using Microsoft.AspNet.Identity.EntityFramework;

namespace WashMyCar.API.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Cellphone { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Detailer Detailer { get; set; }
    }
}