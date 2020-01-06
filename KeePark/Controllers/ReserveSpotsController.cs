    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeePark.Models;
using KeePark.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace KeePark.Controllers
{
    public class ReserveSpotsController : Controller
    {
        private readonly UserManager<GeneralUser> _userManager;
        private readonly KeeParkContext _context;
        private readonly IdentityContext _identitycontext;

        public ReserveSpotsController(KeeParkContext context, IdentityContext identitycontext, UserManager<GeneralUser> userManager)
        {
            _context = context;
            _identitycontext = identitycontext;
            _userManager = userManager;
        }


        // GET: ReserveSpots
        public async Task<IActionResult> Index(string spotsName, DateTime resDate, string carNumber)
        {
               
            var userid = _userManager.GetUserId(HttpContext.User);
            GeneralUser user = _userManager.FindByIdAsync(userid).Result;
            var res = (from reservations in _context.ReserveSpot
                       where reservations.UserID == user.UID
                       select reservations).Include(r => r.Spot);

            var reserves = from r in res select r;

            // Smart Search
            if (!String.IsNullOrEmpty(spotsName))
            {
                reserves = reserves.Where(a => a.Spot.SpotName.Contains(spotsName));
            }

            if (resDate != DateTime.MinValue)
            {
                reserves = reserves.Where(a => a.ReservationDate.Date.Equals(resDate.Date));
            }

            if (!String.IsNullOrEmpty(carNumber))
            {
                reserves = reserves.Where(a => a.carNumber.Equals(carNumber));
            }

            return View(await reserves.ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> List(string userId, string spotsName, DateTime resDate)
        {

                var keeParkContext = (from allres in _context.ReserveSpot
                                     select allres).Include(r => r.Spot);
                var reserves = from r in keeParkContext select r;
            // Smart Search
            if (!String.IsNullOrEmpty(userId))
            {
                reserves = reserves.Where(a => a.UserID.Equals(userId));
            }
            if (!String.IsNullOrEmpty(spotsName))
                {
                reserves = reserves.Where(a => a.Spot.SpotName.Contains(spotsName));
                }

                if (resDate != DateTime.MinValue)
                {
                reserves = reserves.Where(a => a.ReservationDate.Date.Equals(resDate.Date));
                }

               


                return View(await reserves.ToListAsync());
            
          
        }


        public IActionResult FutureOrders()
        {
            var userid = _userManager.GetUserId(HttpContext.User);
            GeneralUser user = _userManager.FindByIdAsync(userid).Result;
            var reserve = (from reservations in _context.ReserveSpot
                           where reservations.UserID == user.UID && reservations.ReservationDate > DateTime.Now
                           select reservations).Include(r => r.Spot);
            return View(reserve);
        }


        // GET: ReserveSpots/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserveSpot = await _context.ReserveSpot
                .Include(r => r.Spot)
                .FirstOrDefaultAsync(m => m.ReserveSpotID == id);
            if (reserveSpot == null)
            {
                return NotFound();
            }

            return View(reserveSpot);
        }

        public IActionResult Create([FromRoute]int parkingSpotID)
        {
            //getting car number for the view
            var carN = (from mycar in _identitycontext.GeneralUser
                        where mycar.Email == User.Identity.Name
                        select mycar.CarNumber).FirstOrDefault();
            //getting user id
            var userid = (from myid in _identitycontext.GeneralUser
                          where myid.Email == User.Identity.Name
                          select myid.UID).FirstOrDefault();

            var viewModel = new ReserveSpot
            {
                carNumber = carN,
                ReservationDate = System.DateTime.Now,
                CreatedOn = System.DateTime.Now,
                SpotID = parkingSpotID

            };

            return View(viewModel);
        }

        // POST: ReserveSpots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("ReserveSpots/Create/{parkingSpotID}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReserveSpotID,UserID,SpotID,CreatedOn,ReservationDate,ReservationHour,Duration,carNumber")] ReserveSpot reserveSpot, [FromRoute]int parkingSpotID)
        {
            if (ModelState.IsValid)
            {

                //checks if the reservation date didn't pass or if the the reservation date is today
                if ((reserveSpot.ReservationDate.Date >= DateTime.Today))
                {
                    //checks if the reservation hour is valid for today's date
                    if ((reserveSpot.ReservationHour <= System.DateTime.Now.Hour) && ((reserveSpot.ReservationDate.Date == DateTime.Today)))
                        return RedirectToAction(nameof(InvalidHour));


                    //getting the id of current user
                    var currentUser = (from userID in _identitycontext.GeneralUser
                                       where userID.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)
                                       select userID.UID).FirstOrDefault();




                    reserveSpot.ReserveSpotID = Guid.NewGuid();
                    reserveSpot.CreatedOn = System.DateTime.Now;
                    reserveSpot.Spot = _context.ParkingSpot.FirstOrDefault(u => u.ParkingSpotID == parkingSpotID);
                    reserveSpot.UserID = currentUser;



                    //get all the reservations for the wanted day for a specific spot spot
                    var myreservations = (from reservations in _context.ReserveSpot
                                          where reservations.Spot.ParkingSpotID == parkingSpotID && reservations.ReservationDate == reserveSpot.ReservationDate  //replace
                                          select reservations);

                    //validation that the reservation made for the desired date
                    if (reserveSpot.ReservationHour + reserveSpot.Duration > 24)
                    {
                        return RedirectToAction(nameof(SorryChooseDurationForTheDayYouSelected));
                    }


                    foreach (var r in myreservations)
                    {
                        //case1: check if the desired res hour is between an already reserved time range
                        if ((reserveSpot.ReservationHour < r.ReservationHour + r.Duration && reserveSpot.ReservationHour >= r.ReservationHour))
                        {
                            return RedirectToAction(nameof(SorrySpotIsTaken));
                        }

                        //case2: the desired res hour is an availble hour but the duration slip into other reservation and its end time is  before the already existing reservation fullfiled
                        if ((reserveSpot.ReservationHour + reserveSpot.Duration <= r.ReservationHour + r.Duration) && (reserveSpot.ReservationHour + reserveSpot.Duration > r.ReservationHour))
                        {
                            return RedirectToAction(nameof(SorrySpotIsTaken));
                        }
                        //case3: the desired res hour is an availble hour but the duration slip into other reservation
                        if ((reserveSpot.ReservationHour <= r.ReservationHour) && (reserveSpot.ReservationHour + reserveSpot.Duration >= r.ReservationHour + r.Duration))
                        {
                            return RedirectToAction(nameof(SorrySpotIsTaken));
                        }

                    }


                    reserveSpot.Spot.NunOfOrders++;
                    _context.Add(reserveSpot);
                    await _context.SaveChangesAsync();

                    // finding current user by name
                    var uName = User.Identity.Name;
                    var thisUser = await _identitycontext.GeneralUser.FirstOrDefaultAsync(u => u.UserName.Equals(uName));

                    // validation thisUser really exists
                    if (thisUser == null)
                        return NotFound();

                    // this section is to save the history of spots reserved to serve the ML algo
                    string uHistory = thisUser.History;
                    if (!string.IsNullOrEmpty(uHistory))
                    {
                        uHistory += ",";
                    }
                    uHistory += reserveSpot.SpotID.ToString();
                    thisUser.History = uHistory;

                    try
                    {
                        _identitycontext.Update(thisUser);
                        await _identitycontext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw new Exception("error");
                    }

                    return RedirectToAction(nameof(Index));

                }

                else
                {
                    return RedirectToAction(nameof(InvalidDate));

                }



            }
            return View(reserveSpot);

        }


        public IActionResult InvalidHour()
        {
            return View();
        }

        public IActionResult InvalidDate()
        {
            return View();
        }

        public IActionResult SorrySpotIsTaken()
        {
            return View();
        }

        public IActionResult SorryChooseDurationForTheDayYouSelected()
        {
            return View();
        }

        // GET: ReserveSpots/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserveSpot = await _context.ReserveSpot.FindAsync(id);
            if (reserveSpot.ReservationDate.Date < DateTime.Today)
            {
                return RedirectToAction(nameof(ReservationAlreadyFullfiled));
            }
            if (reserveSpot == null)
            {
                return NotFound();
            }
            ViewData["SpotID"] = new SelectList(_context.ReserveSpot, "SpotID", "SpotID", reserveSpot.SpotID);
            ViewData["UserID"] = new SelectList(_context.ReserveSpot, "UserID", "UserID", reserveSpot.UserID);
            return View(reserveSpot);
        }

        // POST: ReserveSpots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReserveSpotID,UserID,SpotID,CreatedOn,ReservationDate,ReservationHour,Duration,carNumber")] ReserveSpot reserveSpot)
        {
            if (id != reserveSpot.ReserveSpotID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
         
                if ((reserveSpot.ReservationDate.Date >= DateTime.Today))
                {
                    //checks if the reservation hour is valid for today's date
                    if ((reserveSpot.ReservationHour <= System.DateTime.Now.Hour) && ((reserveSpot.ReservationDate.Date == DateTime.Today)))
                        return RedirectToAction(nameof(InvalidHour));

                    try
                    {
                        var userid = (from users in _identitycontext.GeneralUser
                                      where users.UserName == User.Identity.Name
                                      select users.UID).FirstOrDefault();
                        var spotid = (from myid in _context.ReserveSpot
                                      where myid.ReserveSpotID == id
                                      select myid.SpotID).FirstOrDefault();

                        reserveSpot.UserID = userid;
                        reserveSpot.Spot = _context.ParkingSpot.FirstOrDefault(u => u.ParkingSpotID == spotid);
                        reserveSpot.SpotID = reserveSpot.Spot.ParkingSpotID;
                        reserveSpot.CreatedOn = System.DateTime.Now;


                        var res = from reserve in _context.ReserveSpot
                                  where reserve.ReserveSpotID != id
                                  select reserve;

                        var spots = from sp in res
                                    where sp.Spot.ParkingSpotID == reserveSpot.SpotID && sp.ReservationDate == reserveSpot.ReservationDate
                                    select sp;

                        //validation that the reservation made for the desired date
                        if (reserveSpot.ReservationHour + reserveSpot.Duration > 24)
                        {
                            return RedirectToAction(nameof(SorryChooseDurationForTheDayYouSelected));
                        }

                        foreach (var r in spots)
                        {

                            //case1: check if the desired res hour is between an already reserved time range
                            if ((reserveSpot.ReservationHour < r.ReservationHour + r.Duration && reserveSpot.ReservationHour >= r.ReservationHour))
                            {
                                return RedirectToAction(nameof(SorrySpotIsTaken));
                            }

                            //case2: the desired res hour is an availble hour but the duration slip into other reservation and its end time is  before the already existing reservation fullfiled
                            if ((reserveSpot.ReservationHour + reserveSpot.Duration <= r.ReservationHour + r.Duration) && (reserveSpot.ReservationHour + reserveSpot.Duration > r.ReservationHour))
                            {
                                return RedirectToAction(nameof(SorrySpotIsTaken));
                            }
                            //case3: the desired res hour is an availble hour but the duration slip into other reservation
                            if ((reserveSpot.ReservationHour <= r.ReservationHour) && (reserveSpot.ReservationHour + reserveSpot.Duration >= r.ReservationHour + r.Duration))
                            {
                                return RedirectToAction(nameof(SorrySpotIsTaken));
                            }

                        }


                        _context.Update(reserveSpot);
                        await _context.SaveChangesAsync();

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ReserveSpotExists(reserveSpot.ReserveSpotID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }


                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    return RedirectToAction(nameof(InvalidDate));

                }

            }
            ViewData["SpotID"] = new SelectList(_context.ReserveSpot, "SpotID", "SpotID", reserveSpot.SpotID);
            ViewData["UserID"] = new SelectList(_context.ReserveSpot, "UserID", "UserID", reserveSpot.UserID);
            return View(reserveSpot);
        }

        // GET: ReserveSpots/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var reserveSpot = await _context.ReserveSpot
                .Include(r => r.Spot)
                .FirstOrDefaultAsync(m => m.ReserveSpotID == id);
            if (reserveSpot == null)
            {
                return NotFound();
            }


            return View(reserveSpot);
        }

        // POST: ReserveSpots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var reserveSpot = await _context.ReserveSpot.FindAsync(id);
            //cannot delete a fullfiled res
            if ((reserveSpot.ReservationDate.Date < DateTime.Today) || ((reserveSpot.ReservationDate.Date == DateTime.Today) && (reserveSpot.ReservationHour <= System.DateTime.Now.Hour)))
            {
                return RedirectToAction(nameof(CanNotDelete));
            }

            var spot = (from res in _context.ReserveSpot
                        where res.ReserveSpotID == id
                        select res.Spot).FirstOrDefault();
            spot.NunOfOrders--;
            _context.ReserveSpot.Remove(reserveSpot);
            await _context.SaveChangesAsync();
            if(!User.IsInRole("Administrator"))
             return RedirectToAction(nameof(Index));

            return (RedirectToAction(nameof(List)));
        }

        private bool ReserveSpotExists(Guid id)
        {
            return _context.ReserveSpot.Any(e => e.ReserveSpotID == id);
        }


        public IActionResult Weather()
        {
            return View();
        }
        public IActionResult CanNotDelete()
        {
            return View();
        }
        public IActionResult ReservationAlreadyFullfiled()
        {
            return View();
        }


    }
}