
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeePark.Models;
using Microsoft.AspNetCore.Mvc;
using KeePark.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace KeePark.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StatisticsController : Controller
    {
        private readonly KeeParkContext _context;
        private readonly IdentityContext _identitycontext;

        public StatisticsController(KeeParkContext context, IdentityContext identitycontext)
        {
            _context = context;
            _identitycontext = identitycontext;
        }

        //Counter of orders by user
        public IActionResult UserStat()
        {
            //getting reservation of the curreny year
            var t = from u in _context.ReserveSpot
                    where u.CreatedOn.Year == System.DateTime.Today.Year
                    select u;
            //grouping orders by users 
            var groupJoinQuery =
            from orders in t
            join users in _context.ReserveSpot on orders.UserID equals users.UserID into uorderGroup
            from uorder in uorderGroup
            group uorder by uorder.UserID into g
            select new { Name = g.Key, Count = g.Count() };

            //from the groupjoin we ordery by count ant take first 5
            var list = (from mygroup in groupJoinQuery
                        orderby mygroup.Count descending
                        select mygroup).Take(5);

            Dictionary<string, int> map;
            //name=uid, count=num of orders
            map = list.ToList().ToDictionary(a => a.Name, a => a.Count);
            //viewbag is a way to pass data from controller to view, we want to pass a map(key& value)
            //its a dynamic property
            ViewBag.Map = map;
            return View();
        }

        public IActionResult PopularSpot()
        {
            return View();
        }
        public string GetData()
        {
            var numOfOrders = from parkingSpot in _context.ParkingSpot
                              join reservtion in _context.ReserveSpot on parkingSpot.ParkingSpotID equals reservtion.SpotID into numOrderGroup
                              from numorder in numOrderGroup
                              group numorder by numorder.SpotID into g
                              from parkingSpotName in _context.ParkingSpot
                              where parkingSpotName.ParkingSpotID == g.Key
                              select new { Name = parkingSpotName.SpotName, Count = g.Count() };

            var spots = (from s in numOfOrders
                         orderby s.Count descending
                         select s).Take(5);


            StringBuilder output = new StringBuilder();
            output.Append("name,value\n");

            foreach (var item in spots)
            {
                output.Append(item.Name + "," + item.Count + "\n");
            }

            return output.ToString();
        }
    }
}