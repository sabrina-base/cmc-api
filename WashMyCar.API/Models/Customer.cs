using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WashMyCar.API.Models
{
    public class Customer : Person  
    {
        public Customer()
        {
            Appointments = new Collection<Appointment>();
        }

        // Scalar properties
        public int CustomerId { get; set; }

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual User User { get; set; }
    }
}