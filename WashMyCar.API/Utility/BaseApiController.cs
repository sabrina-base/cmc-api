using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WashMyCar.API.Data;
using WashMyCar.API.Models;

namespace WashMyCar.API.Utility
{
    public abstract class BaseApiController : ApiController
    {
        protected WashMyCarDataContext db = new WashMyCarDataContext();

        public User CurrentUser
        {
            get
            {
                var userName = base.User.Identity.Name;
                var user = db.Users.FirstOrDefault(u => u.UserName == userName);

                return user;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}