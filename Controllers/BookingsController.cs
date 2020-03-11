﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        // GET: Bookings/Create
        [Authorize]
        public IActionResult Create(int id)
        {
            Cottage c1 = new Cottage();
            ViewData["cottageid"] = id;
            var cottage = _context.Cottage
                .FirstOrDefault(m => m.CottageId == id);

            ViewData["cottagename"] = cottage.Name;
            ViewData["cottageprice"] = cottage.Price;

            //ViewData["CottageId"] = new SelectList(_context.Cottage, "CottageId", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            /*ViewData["Price"] = from m in _context.Cottage.Include(c => c.Price)
                                select m;*/
            //FORTSÄTT HÄR ViewData["Price"] = _context.Cottage.Include(b => b.Price);

            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,UserId,DateArrival,DateLeaving,TotalPrice")] Booking booking, int id)
        {
            if (ModelState.IsValid)
            {
                // Räkna ut totalpris
                int days = (booking.DateLeaving.Date - booking.DateArrival.Date).Days;

                var cottage = await _context.Cottage
                .FirstOrDefaultAsync(m => m.CottageId == booking.CottageId);

                //Läser ut pris för aktuell stuga
                var cottagePrice = Convert.ToInt32(Request.Form["price"]);

                int TotalPrice = cottagePrice;

                //Pris baserat på hur många dagar som väljs
                if (days > 2 && days <= 4)
                {
                    TotalPrice = cottagePrice + 1000;
                }
                else if (days > 4)
                {
                    TotalPrice = cottagePrice + 2000;
                }

                //Lagrar värde för totalpris innan lagring i databas
                booking.TotalPrice = TotalPrice;
                booking.CottageId = id;

                //Den stuga med det id som finns här ska få cottage.IsBooked = true;
                if (cottage.CottageId == booking.CottageId)
                {
                    cottage.IsBooked = true;
                }

                _context.Add(booking);
                //_context.Add(cottage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CottageId"] = new SelectList(_context.Cottage, "CottageId", "Name", booking.CottageId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserN", booking.UserId);
            return View(booking);
        }
        /*public async Task<IActionResult> Create([Bind("BookingId,UserId,CottageId,DateArrival,DateLeaving,TotalPrice")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CottageId"] = new SelectList(_context.Cottage, "CottageId", "Description", booking.CottageId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }*/

        // GET: Bookings/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CottageId"] = new SelectList(_context.Cottage, "CottageId", "Description", booking.CottageId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,UserId,CottageId,DateArrival,DateLeaving,TotalPrice")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
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
            ViewData["CottageId"] = new SelectList(_context.Cottage, "CottageId", "Description", booking.CottageId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", booking.UserId);
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
