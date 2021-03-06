﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using smalandscamping.Data;
using smalandscamping.Models;
using smalandscamping.ViewModels;

namespace smalandscamping.Controllers
{
    public class CottagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public CottagesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Cottages
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cottage.ToListAsync());
        }

        // GET: Cottages/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottage = await _context.Cottage
                .FirstOrDefaultAsync(m => m.CottageId == id);
            if (cottage == null)
            {
                return NotFound();
            }

            return View(cottage);
        }

        // GET: Cottages/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cottages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CottageId,Name,Price,NumberOfGuest,AnimalsAllowed,Description,IsBooked,Photo")] CottageCreateViewModel cottageModel)
        {
            if (ModelState.IsValid)
            {
                //Kod för bilduppladdning
                string cottageFileName = null;
                if(cottageModel.Photo != null)
                {
                    //Mapp där bilderna lagras
                    string imagesFolder = Path.Combine(hostingEnvironment.WebRootPath, "img/cottageimg");

                    //Filnamn
                    cottageFileName = Guid.NewGuid().ToString() + "_" + cottageModel.Photo.FileName;

                    //Sökväg till bild
                    string filePath = Path.Combine(imagesFolder, cottageFileName);

                    //Kopierar över till server
                    cottageModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                //Lägger till alla värden
                Cottage newCottage = new Cottage
                {
                    Name = cottageModel.Name,
                    Price = cottageModel.Price,
                    NumberOfGuest = cottageModel.NumberOfGuest,
                    AnimalsAllowed = cottageModel.AnimalsAllowed,
                    Description = cottageModel.Description,
                    IsBooked = cottageModel.IsBooked,
                    PhotoPath = cottageFileName
                };

                _context.Add(newCottage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Cottages/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottage = await _context.Cottage.FindAsync(id);

            if (cottage == null)
            {
                return NotFound();
            }

            return View(cottage);
        }

        // POST: Cottages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CottageId,Name,Price,NumberOfGuest,AnimalsAllowed,Description,IsBooked,Photo")] CottageCreateViewModel cottageModel)
        {
            if (id != cottageModel.CottageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Tilldelar värden för uppdatering av stuga
                    var cottage = await _context.Cottage
                    .FirstOrDefaultAsync(m => m.CottageId == id);

                    cottage.Name = cottageModel.Name;
                    cottage.Price = cottageModel.Price;
                    cottage.NumberOfGuest = cottageModel.NumberOfGuest;
                    cottage.AnimalsAllowed = cottageModel.AnimalsAllowed;
                    cottage.Description = cottageModel.Description;
                    cottage.IsBooked = cottageModel.IsBooked;
                    cottage.PhotoPath = cottage.PhotoPath;


                    _context.Update(cottage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CottageExists(cottageModel.CottageId))
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
            return View();
        }

        // GET: Cottages/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottage = await _context.Cottage
                .FirstOrDefaultAsync(m => m.CottageId == id);
            if (cottage == null)
            {
                return NotFound();
            }

            return View(cottage);
        }

        // POST: Cottages/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cottage = await _context.Cottage.FindAsync(id);
            _context.Cottage.Remove(cottage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CottageExists(int id)
        {
            return _context.Cottage.Any(e => e.CottageId == id);
        }
    }
}
