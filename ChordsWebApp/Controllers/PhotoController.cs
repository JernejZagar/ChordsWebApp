using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChordsWebApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using static ChordsWebApp.ChordsWebAppContext;

namespace ChordsWebApp
{
    public class PhotoController : Controller
    {
        private readonly ChordsWebAppContext _context;

        public PhotoController(ChordsWebAppContext context)
        {
            _context = context;
        }

        // GET: Photo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Photo.ToListAsync());
        }

        // GET: Photo/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            // Prepare photo
            if (photo.ArtistPhoto != null && photo.ArtistPhoto.Length > 0)
            {
                var base64 = Convert.ToBase64String(photo.ArtistPhoto);
                var imgSrc = String.Format($"data:image/gif;base64,{base64}");
                ViewBag.PhotoSrc = imgSrc;
            }
            if (photo.ArtistThumb != null && photo.ArtistThumb.Length > 0)
            {
                var base64 = Convert.ToBase64String(photo.ArtistThumb);
                var thumbSrc = String.Format($"data:image/gif;base64,{base64}");
                ViewBag.ThumbSrc = thumbSrc;
            }

            return View(photo);
        }

        // GET: Photo/Create
        public async Task<IActionResult> CreateAsync()
        {
            var chords = await _context.Chords.ToListAsync();
            var photos = await _context.Photo.ToListAsync();
            List<string> artists = chords.Distinct().Select(x => x.Artist).ToList();
            List<string> artistsEx = photos.Distinct().Select(x => x.Artist).ToList();
            artists = artists.Except(artistsEx).ToList();
            ViewBag.AllArtists = artists;

            return View();
        }

        // POST: Photo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Artist,ArtistPhoto,ArtistThumb")] Photo photo, IFormFile artistPhoto)
        {

            if (ModelState.IsValid)
            {
                // Handle the photo
                HandlePhoto<Photo>(photo, artistPhoto, 350);
                photo.Id = Guid.NewGuid();
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var chords = await _context.Chords.ToListAsync();
            var photos = await _context.Photo.ToListAsync();
            List<string> artists = chords.Distinct().Select(x => x.Artist).ToList();
            List<string> artistsEx = photos.Distinct().Select(x => x.Artist).ToList();
            artists = artists.Except(artistsEx).ToList();
            ViewBag.AllArtists = artists;


            return View(photo);
        }

        private void HandlePhoto<T>(T @object, IFormFile photo, int thumbWidth)
        {
            if (photo != null)
            {
                if (!AuxiliaryFunctions.ValidImageTypes.Contains(photo.ContentType))
                {
                    ModelState.AddModelError("Photo", "Izberite fotografijo v eni izmed naslednjih oblik: BMP, GIF, JPG, or PNG.");
                }
                else
                {
                    using (var reader = new BinaryReader(photo.OpenReadStream()))
                    {
                        if (@object is Photo)
                        {
                            (@object as Photo).ArtistPhoto = reader.ReadBytes((int)photo.Length);
                            (@object as Photo).ArtistThumb = AuxiliaryFunctions.CreateThumbnail((@object as Photo).ArtistPhoto, thumbWidth);
                        }
                    }
                }
            }
        }

        // GET: Photo/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
        }

        // POST: Photo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Artist,ArtistPhoto,ArtistThumb")] Photo photo, IFormFile artistPhoto)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle the photo
                    HandlePhoto<Photo>(photo, artistPhoto, 350);
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
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
            return View(photo);
        }

        // GET: Photo/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            // Prepare photo
            if (photo.ArtistPhoto != null && photo.ArtistPhoto.Length > 0)
            {
                var base64 = Convert.ToBase64String(photo.ArtistPhoto);
                var imgSrc = String.Format($"data:image/gif;base64,{base64}");
                ViewBag.PhotoSrc = imgSrc;
            }
            if (photo.ArtistThumb != null && photo.ArtistThumb.Length > 0)
            {
                var base64 = Convert.ToBase64String(photo.ArtistThumb);
                var thumbSrc = String.Format($"data:image/gif;base64,{base64}");
                ViewBag.ThumbSrc = thumbSrc;
            }

            return View(photo);
        }

        // POST: Photo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var photo = await _context.Photo.FindAsync(id);
            _context.Photo.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(Guid id)
        {
            return _context.Photo.Any(e => e.Id == id);
        }
    }
}
