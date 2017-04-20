using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace WashMyCar.API.Models
{
    public class Person
    {
        //scalar properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Cellphone { get; set; }
        public string Address { get; set; }
        public DbGeography Location { get; set; }
    }
}