using Microsoft.AspNet.Identity.EntityFramework;

namespace WashMyCar.API.Models
{
    public class User : IdentityUser
    {
        public virtual Customer Customer { get; set; }
        public virtual Detailer Detailer { get; set; }
    }
}