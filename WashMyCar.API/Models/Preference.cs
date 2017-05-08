using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Spatial;

namespace WashMyCar.API.Models
{
    public class Preference
    { 
        public int VehicleTypeId { get; set; }
        public DateTime RequestedAppointmentDate { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Services { get; set; }
    }
}