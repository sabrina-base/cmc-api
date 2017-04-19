using System.Data.Entity.Spatial;

namespace WashMyCar.API.Models
{
    public class Person
    {
        // Scalar properties
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Cellphone { get; set; }
        public string Address { get; set; }
        public DbGeography Location { get; set; }
    }
}