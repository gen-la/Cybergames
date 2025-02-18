using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cybergames.Models;

namespace Cybergames.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // List of games, including the hardcoded data
        public List<Game> Games { get; set; } = new()
        {
            new Game { 
                ID = 1, 
                Title = "Cyberpunk 2077", 
                Price = 59.99m, 
                Description = "A futuristic RPG game.",
                Category = "RPG",
                Cover = "https://images.unsplash.com/photo-1557752282-dcc5ff304d4c",
                Img1 = "https://example.com/img1.jpg",
                Img2 = "https://example.com/img2.jpg"
            },
            new Game { 
                ID = 2, 
                Title = "The Witcher 3", 
                Price = 39.99m, 
                Description = "An open-world fantasy RPG.",
                Category = "Fantasy",
                Cover = "https://images.unsplash.com/photo-1552919367-e5c8209e7c02",
                Img1 = "https://example.com/img3.jpg",
                Img2 = "https://example.com/img4.jpg"
            },    
            new Game { 
                ID = 3, 
                Title = "The Witcher 1", 
                Price = 39.99m, 
                Description = "An open-world fantasy RPG.",
                Category = "Fantasy",
                Cover = "https://images.unsplash.com/photo-1552919367-e5c8209e7c02",
                Img1 = "https://example.com/img3.jpg",
                Img2 = "https://example.com/img4.jpg"
            },          
            new Game { 
                ID = 4, 
                Title = "The Witcher 1", 
                Price = 39.99m, 
                Description = "An open-world fantasy RPG.",
                Category = "Fantasy",
                Cover = "https://images.unsplash.com/photo-1552919367-e5c8209e7c02",
                Img1 = "https://example.com/img3.jpg",
                Img2 = "https://example.com/img4.jpg"
            },     
            new Game { 
                ID = 5, 
                Title = "The Witcher 1", 
                Price = 39.99m, 
                Description = "An open-world fantasy RPG.",
                Category = "Fantasy",
                Cover = "https://images.unsplash.com/photo-1552919367-e5c8209e7c02",
                Img1 = "https://example.com/img3.jpg",
                Img2 = "https://example.com/img4.jpg"
            }
        };

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Any logic you want to run when the page loads can go here
        }
    }
}