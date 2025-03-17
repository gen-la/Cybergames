using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cybergames.Pages;

public class Contact : PageModel
{
    public void OnGet()
    {
        // Hantera GET-förfrågan (ladda sidan)
    }

    public IActionResult OnPost()
    {
        // Hantera POST-förfrågan (skicka formuläret)
        // Här kan du lägga till logik för att skicka e-post eller spara meddelandet i en databas
        return RedirectToPage("/ContactConfirmation");
    }
}
