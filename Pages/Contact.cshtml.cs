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
        // (skicka formuläret)
        // 
        return RedirectToPage("/ContactConfirmation");
    }
}
