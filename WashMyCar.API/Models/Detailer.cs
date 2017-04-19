using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WashMyCar.API.Models
{
    public class Detailer : Person
    {
        public Detailer()
        {
            this.DetailerAvailabilities = new Collection<DetailerAvailability>();
            this.DetailerServices = new Collection<DetailerService>();
            this.Appointments = new Collection<Appointment>();
        }

        // Scalar Properties
        public int DetailerId { get; set; }
        public double Rating
        {
            get
            {
                return Math.Round(Appointments.Average(a => a.Rating).GetValueOrDefault(), 1);
            }
        }

        // Navigation Properties
        public virtual ICollection<DetailerAvailability> DetailerAvailabilities { get; set; }
        public virtual ICollection<DetailerService> DetailerServices { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual User User { get; set; }
    }
}