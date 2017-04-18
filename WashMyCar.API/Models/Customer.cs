using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WashMyCar.API.Models
{
    public class Customer : Person  
    {
        public Customer()
        {
            Appointments = new Collection<Appointment>();
        }

        //scalar properties
        public int CustomerId { get; set; }


        // navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual User User { get; set; }
    }
}