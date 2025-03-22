using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cybergames.Models;
using Cybergames.Data;
#nullable disable

namespace Cybergames.Pages.Admin.Games
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        //spelet som ska redigeras, markerat med BindProperty för att automatiskt binda data från formuläret
        [BindProperty]
        public Game Game { get; set; }

        //lista med kategorier som användaren kan välja mellan i en dropdown-meny
        public SelectList CategoryOptions { get; set; }

        //dett körs när sidan laddas med en GET-förfrågan
        public async Task<IActionResult> OnGetAsync(int id)
        {
            //Hitta spelet med det angivna ID i databasen
            Game = await _context.Games.FindAsync(id);

            //Om spelet inte hittades, returnera ett 404-fel (Not Found)
            if (Game == null)
            {
                return NotFound();
            }

            //Skapa en lista med spelkategorier för dropdown-menyn
            CategoryOptions = new SelectList(new[]
            {
                "Action",
                "Adventure",
                "Arcade",
                "Casual",
                "Fighting",
                "FPS",
                "Platform",
                "Puzzle",
                "Racing",
                "RPG",
                "Simulation",
                "Sports"
            });

            //Visa sidan med det hämtade spelet
            return Page();
        }

        //detta körs när formuläret skickas med en POST-förfrågan
        public async Task<IActionResult> OnPostAsync()
        {
            //kontrollera om modellen är giltig (alla valideringsregler uppfylls)
            if (!ModelState.IsValid)
            {
                //om modellen inte är giltig, återskapa kategorilistan och visa sidan igen med felmeddelanden
                CategoryOptions = new SelectList(new[]
                {
                    "Action",
                    "Adventure",
                    "Arcade",
                    "Casual",
                    "Fighting",
                    "FPS",
                    "Platform",
                    "Puzzle",
                    "Racing",
                    "RPG",
                    "Simulation",
                    "Sports"
                });
                return Page();
            }

            //markera spelet som modifierat så att Entity Framework vet att det ska uppdateras
            _context.Attach(Game).State = EntityState.Modified;

            try
            {
                //spara ändringarna till databasen
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //hantera konflikter (när någon annan har ändrat samma spel)
                if (!GameExists(Game.ID))
                {
                    //om spelet inte längre finns, returnera ett 404-fel
                    return NotFound();
                }
                else
                {
                    //om det är ett annat fel
                    throw;
                }
            }

            //omdirigera till indexsidan efter att spelet uppdaterats
            return RedirectToPage("./Index");
        }

        //kontrollera om ett spel med ett visst ID existerar i databasen
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.ID == id);
        }
    }
}