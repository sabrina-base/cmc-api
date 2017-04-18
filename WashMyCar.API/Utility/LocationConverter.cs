using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace WashMyCar.API.Utility
{
    public static class LocationConverter
    {
        public static DbGeography GeocodeAddress(string address)
        {
            var geocoderService = new Geocoder.GeocodeService();

            var location = geocoderService.GeocodeLocation(address);

            return DbGeography.FromText($"POINT({location.Longitude} {location.Latitude})");
        }
    }
}