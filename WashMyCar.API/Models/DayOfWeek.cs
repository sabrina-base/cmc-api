using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WashMyCar.API.Models
{
    public class DayOfWeek
    {
        public DayOfWeek()
        {
            DetailersAvailability = new Collection<DetailerAvailability>();
        }

        //scalar properties
        public int DayOfWeekId { get; set; }
        public string Weekday { get; set; }

        //navigation properties
        public virtual ICollection<DetailerAvailability> DetailersAvailability { get; set; }
    }
}