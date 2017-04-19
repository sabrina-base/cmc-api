using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WashMyCar.API.Models
{
    public class Detailer : Person
    {
        public Detailer()
        {
            DetailerAvailabilities = new Collection<DetailerAvailability>();
            Appointments = new Collection<Appointment>();
            DetailerServices = new Collection<DetailerService>();
        }

        //Scalar Properties
        public int DetailerId { get; set; }
        public double Rating
        {
            get
            {
                return Math.Round(Appointments.Average(a => a.Rating).GetValueOrDefault(), 1);
            }
        }

        //Navigation Properties
        public virtual ICollection<DetailerAvailability> DetailerAvailabilities { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<DetailerService> DetailerServices { get; set; }
        public virtual User User { get; set; }
    }
}