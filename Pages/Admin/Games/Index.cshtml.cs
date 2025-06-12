using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cybergames.Models;
using Cybergames.Data;
using Microsoft.AspNetCore.Authorization;

namespace Cybergames.Pages.Admin.Games
{
    //endast anv�ndare som uppfyller "AdminOnly"-policyn kan komma �t denna sida
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        //variabel som inneh�ller databassammanhanget
        private readonly ApplicationDbContext _context;

        //ta emot databassammanhanget via dependency injection
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public IndexModel(ApplicationDbContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            _context = context;
        }

        //lista som kommer inneh�lla alla spel som ska visas p� sidan
        public IList<Game> Games { get; set; } = default!;

        //egenskap f�r att binda s�kstr�ngen fr�n URL:en
        //SupportsGet = true betyder att v�rdet kan komma fr�n en GET-f�rfr�gan (fr�n URL:en)
        [BindProperty(SupportsGet = true)]
        public string searchString { get; set; }

        public async Task OnGetAsync(string searchString)
        {
            //h�mta alla spel fr�n databasen
            var games = from g in _context.Games
                        select g;

            //om en s�kstr�ng har angets, filtrera spelen baserat p� titel eller kategori
            if (!String.IsNullOrEmpty(searchString))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                games = games.Where(s => s.Title.Contains(searchString) ||
                s.Category.Contains(searchString));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            //h�mta spelen fr�n databasen och lagra dem i Games
            Games = await games.ToListAsync();
        }

        //k�r n�r en POST-f�rfr�gan f�r att ta bort ett spel skickas
        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            //om inget ID angavs, returnera ett 404-fel (Not Found)
            if (id == null)
            {
                return NotFound();
            }

            //hitta spelet med det angivna ID i databasen
            var game = await _context.Games.FindAsync(id);

            //om spelet hittades, ta bort det fr�n databasen
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            //omdirigera tillbaka till samma sida f�r att visa den uppdaterade listan
            return RedirectToPage();
        }
    }
}