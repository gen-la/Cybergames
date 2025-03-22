using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cybergames.Models;
using Cybergames.Data;
using Microsoft.AspNetCore.Authorization;

namespace Cybergames.Pages.Admin.Games
{
    //endast användare som uppfyller "AdminOnly"-policyn kan komma åt denna sida
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        //variabel som innehåller databassammanhanget
        private readonly ApplicationDbContext _context;

        //ta emot databassammanhanget via dependency injection
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        //lista som kommer innehålla alla spel som ska visas på sidan
        public IList<Game> Games { get; set; } = default!;

        //egenskap för att binda söksträngen från URL:en
        //SupportsGet = true betyder att värdet kan komma från en GET-förfrågan (från URL:en)
        [BindProperty(SupportsGet = true)]
        public string searchString { get; set; }

        public async Task OnGetAsync(string searchString)
        {
            //hämta alla spel från databasen
            var games = from g in _context.Games
                        select g;

            //om en söksträng har angets, filtrera spelen baserat på titel eller kategori
            if (!String.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.Title.Contains(searchString) ||
                s.Category.Contains(searchString));
            }

            //hämta spelen från databasen och lagra dem i Games
            Games = await games.ToListAsync();
        }

        //kör när en POST-förfrågan för att ta bort ett spel skickas
        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            //om inget ID angavs, returnera ett 404-fel (Not Found)
            if (id == null)
            {
                return NotFound();
            }

            //hitta spelet med det angivna ID i databasen
            var game = await _context.Games.FindAsync(id);

            //om spelet hittades, ta bort det från databasen
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }

            //omdirigera tillbaka till samma sida för att visa den uppdaterade listan
            return RedirectToPage();
        }
    }
}