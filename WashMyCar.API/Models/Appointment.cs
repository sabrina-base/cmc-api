using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WashMyCar.API.Models
{
    public class Appointment 
    {
        public Appointment()
        {
            this.AppointmentServices = new Collection<AppointmentService>();
        }

        //scalar properties
        public int AppointmentId { get; set; }
        public int VehicleTypeId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int CustomerId { get; set; }
        public int DetailerId { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public int? Rating { get; set; }

        // computed properties
        public decimal TotalCost
        {
            get
            {
                return AppointmentServices.Sum(s => s.Service.Cost);
            }
        }

        // Navigation Properties
        public virtual VehicleType VehicleType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Detailer Detailer { get; set; }

        public virtual ICollection<AppointmentService> AppointmentServices { get; set; }
    }
}