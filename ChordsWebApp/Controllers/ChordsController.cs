using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChordsWebApp.Models;

namespace ChordsWebApp
{
    public class ChordsController : Controller
    {
        private readonly ChordsWebAppContext _context;

        public ChordsController(ChordsWebAppContext context)
        {
            _context = context;
        }

        // GET: Chords
        public async Task<IActionResult> Index()
        {
            var results = await _context.Chords.ToListAsync();
            return View(results);
        }

        // GET: Chords/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chords = await _context.Chords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chords == null)
            {
                return NotFound();
            }

            return View(chords);
        }

        // GET: Chords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Artist,Song,Capo,Key,Content,DateAdded,DateEdited,Uploader")] Chords chords)
        {
            if (ModelState.IsValid)
            {
                chords.Id = Guid.NewGuid();
                // ob kreiranju nastavimo oba datuma na trenutni čas
                chords.DateAdded = DateTime.Now;
                chords.DateEdited = DateTime.Now;
                _context.Add(chords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chords);
        }

        // GET: Chords/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chords = await _context.Chords.FindAsync(id);
            if (chords == null)
            {
                return NotFound();
            }
            return View(chords);
        }

        // POST: Chords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Artist,Song,Capo,Key,Content,DateAdded,DateEdited,Uploader")] Chords chords)
        {
            if (id != chords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // popravimo datum spremembe
                    chords.DateEdited = DateTime.Now;
                    _context.Update(chords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChordsExists(chords.Id))
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

            return View(chords);
        }

        // GET: Chords/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chords = await _context.Chords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chords == null)
            {
                return NotFound();
            }

            return View(chords);
        }

        // POST: Chords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var chords = await _context.Chords.FindAsync(id);
            _context.Chords.Remove(chords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChordsExists(Guid id)
        {
            return _context.Chords.Any(e => e.Id == id);
        }
    }
}
