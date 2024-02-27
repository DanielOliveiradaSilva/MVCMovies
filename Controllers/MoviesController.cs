using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        // GET: Movies
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var moviesWithStudios = await _context.Movie
            .Include(m => m.Studio)
            .Include(m => m.Artists)
            .ToListAsync();
            return View(moviesWithStudios);
        }


        // GET: Movies/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.Include(m => m.Studio).Include(m => m.Artists)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize]
       
        public IActionResult Create()
        {
            // Obtém a lista de estúdios do banco de dados
            var studios = _context.Studio.ToList();

            ViewBag.Studios = studios;

            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price, StudioId")] Movie movie)
        {

            if (ModelState.IsValid)
            {

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Studio)
                .Include(m => m.Artists)
                .FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Obtain the list of artists from the database
            var Artists = _context.Artist.ToListAsync();
            var Studios = _context.Artist.ToListAsync();
            var Movie = movie.Artists.ToList();
            ViewBag.MovieArtist =  Movie;
            ViewBag.Studios = Studios;
            ViewBag.Artists = Artists; 
    
            return View(movie);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,ReleaseDate,Genre,Price,StudioId")] Movie movie)
        {
          

            // Repopula a lista de artistas e estúdios em caso de erros de validação
            var artists = _context.Artist.ToList();
            ViewBag.AllArtists = new MultiSelectList(artists, "ArtistId", "Name", movie.Artists.Select(a => a.ArtistId).ToList());

            var studios = _context.Studio.ToList();
            ViewBag.Studios = new SelectList(studios, "StudioId", "Name", movie.StudioId);

            return View(movie);
        }

       
        // POST: Movies/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movie?.Any(e => e.MovieId == id)).GetValueOrDefault();
        }
    }
}
