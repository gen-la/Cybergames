// Services/CartService.cs
using Cybergames.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Linq;

public class CartService
{
    private const string CartSessionKey = "ShoppingCart"; // Nyckel för att lagra kundvagnen i sessionen
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Konstruktor för att injicera IHttpContextAccessor, vilket möjliggör åtkomst till sessionen
    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Hämtar kundvagnen från sessionen eller skapar en ny om ingen finns
    public ShoppingCart GetCart()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var cart = session.GetObject<ShoppingCart>(CartSessionKey);
        
        if (cart == null)
        {
            cart = new ShoppingCart();
            session.SetObject(CartSessionKey, cart); // Spara den nya kundvagnen i sessionen
        }
        
        return cart;
    }

    // Sparar kundvagnen i sessionen
    public void SaveCart(ShoppingCart cart)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.SetObject(CartSessionKey, cart);
    }

    // Metod för att hämta det totala antalet varor i kundvagnen
    public int GetCartItemCount()
    {
        var cart = GetCart();
        return cart.Items.Sum(item => item.Quantity); // Beräknar det totala antalet varor i kundvagnen
    }
}

// Extensionsmetoder för att hantera objekt i sessionen
public static class SessionExtensions
{
    // Sparar ett objekt i sessionen genom att serialisera det till JSON
    public static void SetObject(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    // Hämtar ett objekt från sessionen genom att deserialisera JSON
    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}
