using ShauliBlogProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoogleMaps.LocationServices;

namespace ShauliBlogProject.Controllers
{
    public class MapsController : Controller
    {
        private ShauliBlogContext db = new ShauliBlogContext();

        public class Marker
        {
            public double x;
            public double y;
        };

        public ActionResult About()
        {
            var Map = (from m in db.Maps select m);
            var count = Map.Count();
            Marker[] markersArrayOffice = new Marker[count];
            int z = 0;

            for (int i = 0; i < markersArrayOffice.Length; i++)
                markersArrayOffice[i] = new Marker();

            foreach (var Item in Map)
            {
                string FullAddress = Item.Address.ToString() + ", " + Item.City.ToString() + ", " + Item.Country.ToString();
                var locationService = new GoogleLocationService();
                var point = locationService.GetLatLongFromAddress(FullAddress);
                markersArrayOffice[z].x = point.Latitude;
                markersArrayOffice[z].y = point.Longitude;
                z++;
            }

            ViewBag.markersArrayOffice = markersArrayOffice;

            var FansMap = (from u in db.Maps
                           join b in db.FansClub
                           on u.Country equals b.Country
                           select new { b.Country, b.City, b.Address });

            count = FansMap.Count();
            Marker[] markersArray = new Marker[count];
            for (int i = 0; i < markersArray.Length; i++)
                markersArray[i] = new Marker();

            z = 0;
            foreach (var Item in FansMap)
            {
                string FullAddress = Item.Address.ToString() + ", " + Item.City.ToString() + ", " + Item.Country.ToString();
                var locationService = new GoogleLocationService();
                var point = locationService.GetLatLongFromAddress(FullAddress);
                markersArray[z].x = point.Latitude;
                markersArray[z].y = point.Longitude;
                z++;
            }

            ViewBag.markersArray = markersArray;

            return View();
        }
    }
}