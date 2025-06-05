using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Cybergames;
using Cybergames.Data;
using Cybergames.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    // Lägg till auktorisering för Admin-mappen, så att endast auktoriserade användare kan komma åt sidor i denna mapp
    options.Conventions.AuthorizeFolder("/Admin");
});

// Lägg till DbContext-konfiguration för MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"), // Hämtar anslutningssträngen från appsettings.json
        new MySqlServerVersion(new Version(8, 0, 21)) // Ange din MySQL-serverversion
    )
);

// Lägg till Identity-tjänster för autentisering och hantering av användare
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Använd ApplicationDbContext för att lagra användardata

// Lägg till en auktoriseringspolicy för admin@admin.com, där endast användare med denna e-postadress kan utföra vissa åtgärder
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.Identity.IsAuthenticated && // Kontrollera om användaren är autentiserad
            context.User.HasClaim(c => // Kontrollera om användaren har en specifik e-postadress
                c.Type == System.Security.Claims.ClaimTypes.Email &&
                c.Value == "admin@admin.com")));
});

// Lägg till sessionhantering
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Ange sessionens timeout till 30 minuter
    options.Cookie.HttpOnly = true; // Se till att cookien endast är tillgänglig via HTTP (och inte via JavaScript)
    options.Cookie.IsEssential = true; // Markera sessionens cookie som "essential", så den inte blockeras av integritetsinställningar
});

// Lägg till HTTPContextAccessor (krävs för att CartService ska kunna använda sessionen)
builder.Services.AddHttpContextAccessor();

// Registrera CartService som en singleton-tjänst (används för att hantera kundvagnen)
builder.Services.AddSingleton<CartService>();

// Lägg till GameService för att hantera spel (Scoped betyder att det skapas för varje HTTP-begäran)
builder.Services.AddScoped<GameService>();

var app = builder.Build();

// Konfigurera HTTP-begäringspipen (Request pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // Använd en anpassad felhanterare för produktion
    app.UseHsts(); // Använd HSTS (HTTP Strict Transport Security) för att säkerställa HTTPS-anslutningar
}

app.UseHttpsRedirection(); // Omvandla alla HTTP-begäran till HTTPS
app.UseStaticFiles(); // Tillåt åtkomst till statiska filer (t.ex. bilder, CSS, JS)

app.UseRouting(); // Ställ in routing för appen (hantera HTTP-begärningar)

app.UseSession(); // Aktivera sessionshantering (måste vara före UseAuthorization och MapRazorPages)

app.UseAuthentication(); // Aktivera autentisering (nödvändigt för att verifiera användare)
app.UseAuthorization(); // Aktivera auktorisering (nödvändigt för att hantera rättigheter och roller)

app.MapRazorPages(); // Kartlägg Razor Pages (de sidor som användaren kan navigera till)


// Säkerställ att admin-användaren har en e-postclaim vid applikationens start
using (var scope = app.Services.CreateScope())
{
    await AdminSetup.EnsureAdminUserHasEmailClaim(app.Services); // Kontrollera att admin har rätt e-post-claim
}

app.Run(); // Starta applikationen
