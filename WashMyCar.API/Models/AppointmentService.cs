namespace WashMyCar.API.Models
{
    public class AppointmentService
    {
        // Scalar properties
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }

        // Navigation properties
        public virtual Appointment Appointment { get; set; }
        public virtual Service Service { get; set; }
    }
}