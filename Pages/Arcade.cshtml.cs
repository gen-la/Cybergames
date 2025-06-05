using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cybergames.Pages
{
    public class ArcadeModel : PageModel
    {
        private readonly ILogger<ArcadeModel> _logger;

        public ArcadeModel(ILogger<ArcadeModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }

}
