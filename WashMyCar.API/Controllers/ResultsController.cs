using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using WashMyCar.API.Data;
using WashMyCar.API.Models;

namespace WashMyCar.API.Controllers
{
    public class ResultsController : ApiController
    {
        private WashMyCarDataContext db = new WashMyCarDataContext();

        //GET: api/results
        [ResponseType(typeof(Detailer))]
        [Route("api/results/test")]
        public IHttpActionResult GetTest()
        {
            var detailer = db.Detailers.First(x => x.DetailerId == 1);

            return Ok(detailer);
        }

        //GET: api/results
        [Route("api/results")]
        [ResponseType(typeof(Collection<Detailer>))]
        public IHttpActionResult GetResults([FromUri] Preference preference)
        {
            //Get day of week / time / services of desired appointment
            var dayOfWeek = preference.RequestedAppointmentDate.DayOfWeek;
            var time = preference.RequestedAppointmentDate.TimeOfDay;
            var services = preference.Services.Split(',');

            //Get all detailer
            var availableDetailers = db.Detailers
                                 .Where(d => services.All(s => d.DetailerServices
                                                                .Any(ds => ds.Service.ServiceType == s)));

            //Filter by service
            //allDetailers = allDetailers
            //    .Where(x => x.DetailerServices.Contains(services)).ToList();

            //Filter by availability (weekday and time)
            availableDetailers = availableDetailers.Where(x => x.DetailerAvailabilities
                .Any(y => y.DayOfWeek.Weekday == dayOfWeek.ToString()
                    && y.Start <= time.Hours
                    && y.End >= time.Hours));

            // Filter out detailers that have appointments in that time


            return Ok(availableDetailers.Select(d => new
            {
                d.Address,
                d.Cellphone
            }));
        }
    }
}
