using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class CheckoutModel : PageModel
{
    private readonly CartService _cartService;

    public ShoppingCart Cart { get; set; } // Lägg till en property för varukorgen

    [BindProperty]
    public bool ShowConfirmationMessage { get; set; } = false;

    public CheckoutModel(CartService cartService)
    {
        _cartService = cartService;
    }

    public void OnGet()
    {
        Cart = _cartService.GetCart(); // Hämta varukorgen när sidan laddas
        ViewData["CartItemCount"] = Cart.Items.Sum(item => item.Quantity); // Skicka antalet spel till vyn
    }

    public IActionResult OnPost()
    {
        // Töm varukorgen
        var cart = _cartService.GetCart();
        cart.Clear(); // Töm varukorgen
        _cartService.SaveCart(cart); // Spara den uppdaterade (tömda) varukorgen

        // Visa bekräftelsemeddelandet
        ShowConfirmationMessage = true;

        // Uppdatera varukorgen för att visa 0 spel efter att den har tömts
        Cart = _cartService.GetCart();
        ViewData["CartItemCount"] = Cart.Items.Sum(item => item.Quantity);

        // Returnera samma sida för att visa meddelandet
        return Page();
    }
}