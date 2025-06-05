using Microsoft.AspNetCore.Identity;
using Cybergames.Models;
using System.Security.Claims;

namespace Cybergames
{
    public static class AdminSetup
    {
        //kolla så att admin-användaren har en e-post-claim (anspråk)
        //"claim" är en bit information om användaren som används för auktorisering
        public static async Task EnsureAdminUserHasEmailClaim(IServiceProvider serviceProvider)
        {
            //scope för att få tillgång till tjänster som UserManager
            using var scope = serviceProvider.CreateScope();
            //UserManager används för att hantera användare
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //definiera admin-användarens e-postadress
            var adminEmail = "admin@admin.com";
            //sök efter admin-användaren i databasen baserat på e-postadressen
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            //kolla om admin-användaren hittades
            if (adminUser != null)
            {
                //hämtar alla claims (anspråk) som användaren har
                var claims = await userManager.GetClaimsAsync(adminUser);
                //letar efter en specifik e-post-claim med admin-e-postadressen
                var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email && c.Value == adminEmail);

                //om e-post-claim inte finns, lägg till den
                if (emailClaim == null)
                {
                    //lägg till en ny claim av typen Email med admin-e-postadressen
                    await userManager.AddClaimAsync(adminUser, new Claim(ClaimTypes.Email, adminEmail));
                }
            }
        }
    }
}