using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WashMyCar.API.Models
{
    public class DayOfWeek
    {
        public DayOfWeek()
        {
            DetailersAvailability = new Collection<DetailerAvailability>();
        }

        // Scalar properties
        public int DayOfWeekId { get; set; }
        public string Weekday { get; set; }

        // Navigation properties
        public virtual ICollection<DetailerAvailability> DetailersAvailability { get; set; }
    }
}