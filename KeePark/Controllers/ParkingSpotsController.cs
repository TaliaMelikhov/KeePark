using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeePark.Models;
using KeePark.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using KeePark.Data;

namespace KeePark.Controllers
{
    public class ParkingSpotsController : Controller
    {
        private readonly KeeParkContext _context;
        private readonly IdentityContext _identity;
        private readonly UserManager<GeneralUser> UserManager;
        private readonly IHostingEnvironment hostingEnvironment;

        public ParkingSpotsController(IHostingEnvironment hostingEnviroment, KeeParkContext context, IdentityContext identity, UserManager<GeneralUser> userManager)
        {
            this.hostingEnvironment = hostingEnviroment;
            this._identity = identity;
            _context = context;
            UserManager = userManager;
        }

        // GET: ParkingSpots
        public async Task<IActionResult> Index()
        {
            //get current user and show in the index view his spots
            var userid = UserManager.GetUserId(HttpContext.User);
            GeneralUser user = UserManager.FindByIdAsync(userid).Result;
            var spots = (from spot in _context.ParkingSpot
                         where spot.OwnerID == user.UID
                         select spot);
            return View(await spots.ToListAsync());
        }

        // GET: ParkingSpots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpot
                .FirstOrDefaultAsync(m => m.ParkingSpotID == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        public async Task<IActionResult> DetailsFromSearching(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpot
                .FirstOrDefaultAsync(m => m.ParkingSpotID == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        public ActionResult ParkingSpotResult(string spotsName, string spotsAddress, int spotsPrice)
        {
            var spots = from spot in _context.ParkingSpot select spot;

            // Smart Search
            if (!String.IsNullOrEmpty(spotsName))
            {
                spots = spots.Where(a => a.SpotName.Contains(spotsName));
            }

            if (!String.IsNullOrEmpty(spotsAddress))
            {
                spots = spots.Where(a => a.Address.Contains(spotsAddress));
            }

            if (spotsPrice != 0)
            {
                spots = spots.Where(a => (a.Price <= spotsPrice));
            }


            return View(spots.ToList());
        }

        // GET: ParkingSpots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkingSpots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParkingSpotID,SpotName,OwnerID,Address,Price,NunOfOrders,filePath,SpotDescription,parkingPhoto")] ParkingSpotCreate parkingSpot)
        {
            // get the current user personal id
            var currentUser = (from userID in _identity.GeneralUser
                               where userID.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)
                               select userID.UID).FirstOrDefault();
            // with that I am generating the spotID 
            var numOfSpots = 0;
            if (_context.ParkingSpot.Count() != 0)
            {
                numOfSpots = _context.ParkingSpot.Last().ParkingSpotID;
            }
            else
            {
                numOfSpots = _context.ParkingSpot.Count();
            }
                      
            if (ModelState.IsValid)
            {
                string FileName = null;
                if (parkingSpot.parkingPhoto != null)
                {
                    string UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "SpotImages");
                    FileName = Guid.NewGuid().ToString() + "_" + parkingSpot.parkingPhoto.FileName;
                    
                    string filePath = Path.Combine(UploadFolder, FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await parkingSpot.parkingPhoto.CopyToAsync(fileStream);
                    }
                }
                ParkingSpot newSpot = new ParkingSpot
                {
                    ParkingSpotID = numOfSpots + 1,
                    SpotName = parkingSpot.SpotName,
                    OwnerID = currentUser,
                    Address = parkingSpot.Address,
                    Price = parkingSpot.Price,
                    NunOfOrders = 0,
                    SpotDescription = parkingSpot.SpotDescription,
                    filePath = FileName
                };
                _context.Add(newSpot);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = newSpot.ParkingSpotID });
            }
            return View(parkingSpot);
        }

        // GET: ParkingSpots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpot.FindAsync(id);
            if (parkingSpot == null)
            {
                return NotFound();
            }
            return View(parkingSpot);
        }

        // POST: ParkingSpots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParkingSpotID,SpotName,OwnerID,Address,Price,NunOfOrders,filePath,SpotDescription")] ParkingSpot parkingSpot,
            IFormFile file)
        {
            

            if (id != parkingSpot.ParkingSpotID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // if the file did changed than ..
                    if (file != null)
                    {
                        // creating the entire path to the project using the hostingENV
                        string UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "SpotImages");
                        // generating unique filename per file 
                        var FileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        // so the whole path to the file also will be unique using the combine method
                        string filePath = Path.Combine(UploadFolder, FileName);
                        // handeling the saving of a picture using fileStream format
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        parkingSpot.filePath = FileName;
                    }
                    else {
                        // without doing those linq calls we saw that the edit causing disapearnce of data!
                        parkingSpot.filePath = (from spotID in _context.ParkingSpot
                                                where spotID.ParkingSpotID.ToString() == parkingSpot.ParkingSpotID.ToString()
                                                select spotID.filePath).FirstOrDefault();

                        parkingSpot.OwnerID = (from owner in _context.ParkingSpot
                                               where owner.ParkingSpotID.ToString() == parkingSpot.ParkingSpotID.ToString()
                                               select owner.OwnerID).FirstOrDefault();
                    }
                    _context.Update(parkingSpot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingSpotExists(parkingSpot.ParkingSpotID))
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
            return View(parkingSpot);
        }

        // GET: ParkingSpots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpot = await _context.ParkingSpot
                .FirstOrDefaultAsync(m => m.ParkingSpotID == id);
            if (parkingSpot == null)
            {
                return NotFound();
            }

            return View(parkingSpot);
        }

        // POST: ParkingSpots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingSpot = await _context.ParkingSpot.FindAsync(id);
            _context.ParkingSpot.Remove(parkingSpot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingSpotExists(int id)
        {
            return _context.ParkingSpot.Any(e => e.ParkingSpotID == id);
        }
    }
}