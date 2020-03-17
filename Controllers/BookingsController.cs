using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using smalandscamping.Data;
using smalandscamping.Models;

namespace smalandscamping.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //Inloggad användares id
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.userid = userId;

            var applicationDbContext = _context.Booking.Include(b => b.Cottage).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Cottage)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        //Information om stugan som bokas
        public void InfoCottage(int id)
        {
            //Hämtar information från den stuga som har id från argument
            var cottage = _context.Cottage
                .FirstOrDefault(m => m.CottageId == id);

            //Id, namn, pris, max antal gäster och om djur är tillåtna eller inte
            ViewData["cottageid"] = id;
            ViewData["cottagename"] = cottage.Name;
            ViewData["cottageprice"] = cottage.Price;
            ViewData["numberofguests"] = cottage.NumberOfGuest;
            ViewData["animalsallowed"] = cottage.AnimalsAllowed;
        }

        // GET: Bookings/Create
        [Authorize]
        public IActionResult Create(int id)
        {
            //Anropar metod med information med id som argument
            InfoCottage(id);

            return View();
        }

        // POST: Bookings/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,UserId,DateArrival,DateLeaving,TotalPrice")] Booking booking, int id)
        {
            if (ModelState.IsValid)
            {
                // Räknar ut antal dagar baserat på datumen som har valts
                int days = (booking.DateLeaving.Date - booking.DateArrival.Date).Days;

                var cottage = await _context.Cottage
                .FirstOrDefaultAsync(m => m.CottageId == booking.CottageId);

                //Pris för aktuell stuga
                var cottagePrice = Convert.ToInt32(Request.Form["price"]);

                int TotalPrice = cottagePrice;

                //Pris baserat på hur många dagar som väljs
                if (days > 2 && days <= 4)
                {
                    TotalPrice = cottagePrice + 1000;
                }
                else if (days > 4 && days <= 8)
                {
                    TotalPrice = cottagePrice + 2000;
                }
                else if (days > 8)
                {
                    TotalPrice = cottagePrice + 3000;
                }

                //Lagrar värde för totalpris innan lagring i databas
                booking.TotalPrice = TotalPrice;
                booking.CottageId = id;

                //Tilldelar bokningen till den användare som är inloggad
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                booking.UserId = userId;

                //Bokningsstatus för stugan ändras till bokad
                Cottage bookingResult = (from p in _context.Cottage
                             where p.CottageId == id
                             select p).SingleOrDefault();

                bookingResult.IsBooked = true;

                _context.Add(booking);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //Visar information om stugan 
            InfoCottage(id);

            return View(booking);
        }

        // GET: Bookings/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);

            var bookingId = _context.Booking
                .FirstOrDefault(m => m.BookingId == id);

            //Hämta id från stuga där booking id = id
            Booking cottageBooking = (from m in _context.Booking
                                     where m.BookingId == id
                                     select m).SingleOrDefault();

            var cottageId = cottageBooking.CottageId;

            //Hämtar stugans pris
            Cottage cottagePrice = (from m in _context.Cottage
                                    where m.CottageId == cottageId
                                    select m).SingleOrDefault();

            ViewData["cottageprice"] = cottagePrice.Price;
            ViewData["cottageid"] = cottageId;
 
            if (booking == null)
            {
                return NotFound();
            }
            

            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,UserId,DateArrival,DateLeaving,TotalPrice")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Räknar ut antal dagar baserat på datumen som har valts
                    int days = (booking.DateLeaving.Date - booking.DateArrival.Date).Days;

                    //Pris för aktuell stuga
                    var cottagePrice = Convert.ToInt32(Request.Form["price"]);

                    int TotalPrice = cottagePrice;

                    //Pris baserat på hur många dagar som väljs
                    if (days > 2 && days <= 4)
                    {
                        TotalPrice = cottagePrice + 1000;
                    }
                    else if (days > 4 && days <= 8)
                    {
                        TotalPrice = cottagePrice + 2000;
                    }
                    else if (days > 8)
                    {
                        TotalPrice = cottagePrice + 3000;
                    }

                    //Uppdaterar datum för ankomst, datum för hemfärd och det totala priset
                    var bookingU = new Booking { BookingId = id, DateArrival = booking.DateArrival.Date, DateLeaving = booking.DateLeaving.Date, TotalPrice = TotalPrice };
                    _context.Booking.Attach(bookingU);
                    _context.Entry(bookingU).Property(x => x.DateArrival).IsModified = true;
                    _context.Entry(bookingU).Property(x => x.DateLeaving).IsModified = true;
                    _context.Entry(bookingU).Property(x => x.TotalPrice).IsModified = true;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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

            return View(booking);
        }

        // GET: Bookings/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Cottage)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Kod för att ändra bokningsstatus
            Booking cottageBooking = (from m in _context.Booking
                                      where m.BookingId == id
                                      select m).SingleOrDefault();

            var cottageId = cottageBooking.CottageId;

            Cottage bookingResult = (from m in _context.Cottage
                                    where m.CottageId == cottageId
                                    select m).SingleOrDefault();

            //Bokningsstatus ändrad
            bookingResult.IsBooked = false;

            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
