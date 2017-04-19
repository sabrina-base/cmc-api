﻿namespace WashMyCar.API.Models
{
    public class DetailerAvailability
    {
        // Scalar properties
        #region Compound Key
        public int DetailerId { get; set; }
        public int DayOfWeekId { get; set; }
        #endregion

        public float? Start { get; set; }
        public float? End { get; set; }
        public decimal Multiplier { get; set; }

        // Navigation properties
        public virtual Detailer Detailer { get; set; }
        public virtual DayOfWeek  DayOfWeek { get; set; }
    }
}