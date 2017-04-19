namespace WashMyCar.API.Models
{
    public class DetailerService
    {
        // Scalar properties
        #region Compound Key
        public int DetailerId { get; set; }
        public int ServiceId { get; set; }
        #endregion

        public decimal Multiplier { get; set; }

        // Navigation properties
        public virtual Detailer Detailer { get; set; }
        public virtual Service Service { get; set; }
    }
}