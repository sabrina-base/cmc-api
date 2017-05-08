using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WashMyCar.API.Models
{
    public class DetailerService
    {
        // Scalar properties
        #region Compound Key
        public int DetailerId { get; set; }
        public int ServiceId { get; set; }
        #endregion

        // Navigation properties
        public virtual Detailer Detailer { get; set; }
        public virtual Service Service { get; set; }
    }
}