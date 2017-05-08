using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WashMyCar.API.Models
{
    public class Service
    {
        public Service()
        {
            DetailerServices = new Collection<DetailerService>();
            AppointmentServices = new Collection<AppointmentService>();
        }

        //scalar properties
        public int ServiceId { get; set; }
        public string ServiceType { get; set; }
        public decimal Cost { get; set; }

        //navigation properties
        public virtual ICollection<DetailerService> DetailerServices { get; set; }
        public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
    }
}