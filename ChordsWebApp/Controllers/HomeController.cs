using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ChordsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChordsWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // tole dodamo, da se povežemo na bazo, + dodamo tudi v konstruktorju še ta parameter
        private readonly ChordsWebAppContext _context;

        public HomeController(ILogger<HomeController> logger, ChordsWebAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        // dodelana metoda vbistvu GET metoda
        public async Task<IActionResult> IndexAsync(string sortOrder, string searchString)
        {
            
            ViewBag.ArtistSortParam = String.IsNullOrEmpty(sortOrder) ? "artist" : "";
            ViewBag.SongSortParam = String.IsNullOrEmpty(sortOrder) ? "song" : "";
            ViewBag.Searched = searchString;

            //inicializacija baze
            var chordsList = from x in _context.Chords select x;

            //ko vsenemo string in pritisnemo search, se baza sfiltrira po artistu in po song.
            // če ga ne vnesemo se baza ne spremeni
            if (!String.IsNullOrEmpty(searchString))
            {
                chordsList = chordsList.Where(x => x.Artist.Contains(searchString) || x.Song.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "artist":
                    chordsList = chordsList.OrderByDescending(s => s.Artist);
                    break;
                case "song":
                    chordsList = chordsList.OrderBy(s => s.Song);
                    break;
                default:
                    chordsList = chordsList.OrderBy(s => s.Artist);
                    break;
            }

            return View(await chordsList.AsNoTracking().ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // dodana metoda, da se nam pokaže v getChords vsebina iz baze
        public async Task<IActionResult> GetChordsAsync()
        {
            var resultsChord = await _context.Chords.ToListAsync();
            var resultsPhoto = await _context.Photo.ToListAsync();
            
            ViewBag.ResultsChord = resultsChord;
            ViewBag.ResultsPhoto = resultsPhoto;

            return View(resultsChord);
        }

        //tole metodo sem dodal, zato da imam details ločeno od admin vnosov
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chords = await _context.Chords
                .FirstOrDefaultAsync(m => m.Id == id);

            //dodal var photo, in naredil match po Artist!!
            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.Artist == chords.Artist);

            if (chords == null)
            {
                return NotFound();
            }

            // Prepare photo
            if (photo != null)
            {
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
            }

            return View(chords);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
