using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WashMyCar.API.Models
{
    public class VehicleType
    {
        public VehicleType()
        {
            Appointments = new Collection<Appointment>();
        }

        //scalar properties
        public int VehicleTypeId { get; set; }
        public string VehicleSize { get; set; }
        public decimal Multiplier { get; set; }

        //navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}