# 🎮 Cybergames

**Cybergames** is a game webshop with a neon, retro-arcade look, built with **C# and ASP.NET Core (.NET 8)**. It was created as my degree project and covers the full flow of a web shop: browsing games, user accounts, a shopping cart, a (simulated) checkout, a personal game library and an admin area for managing the catalog. There is even a playable arcade game built into the site.


## ✨ Features

**For customers**
- Start page with an animated hero section (background video and a fireworks canvas animation) and a paginated game catalog
- Browse games by category
- Game detail pages with a cover image and a gallery of up to five extra images
- Register and log in with ASP.NET Core Identity (name, address and email are stored on the user)
- Session-based shopping cart (login required to add games)
- Checkout that adds the purchased games to the user's personal library (no real payment is processed)
- "Purchased games" page showing everything the user owns
- **Arcade:** a Breakout game built with HTML canvas and JavaScript, playable in the browser
- About and Contact pages

**For admins**
- Protected admin area under `/Admin/Games`
- Create, edit and delete games
- Search the catalog by title or category


## 🎨 Design

The interface uses a cyberpunk / retro-gaming theme with a dark navy background and neon cyan, magenta and yellow accents, glowing text and glitch effects. It is built on Bootstrap with custom CSS.

## 🛠️ Tech Stack

| Area | Technology |
|------|------------|
| Language | C# |
| Framework | ASP.NET Core, Razor Pages, .NET 8 |
| Authentication | ASP.NET Core Identity with a custom `ApplicationUser` |
| Database | SQLite via Entity Framework Core (code-first migrations) |
| Front-end | Razor views, Bootstrap, custom CSS, JavaScript, jQuery Validation |
| State | Server-side session for the shopping cart |

## 🗃️ Data Model

- **Game**: title, price, description, category, cover image and up to five extra images (URLs)
- **ApplicationUser**: extends `IdentityUser` with first name, last name, address and creation date
- **UserGame**: join entity between users and games (a user's library), with the purchase date
- **ShoppingCart / CartItem**: in-session cart with add, remove, clear and total calculation

## 📁 Project Structure

```
Cybergames/
├── Areas/Identity/Pages/Account/   # Login, logout and registration
├── Data/                           # ApplicationDbContext
├── Migrations/                     # EF Core migrations
├── Models/                         # Game, UserGame, ApplicationUser, cart and GameService
├── Pages/                          # Razor Pages (Index, Category, GameDetails, Cart, Checkout,
│   │                               #   PurchasedGames, Arcade, About, Contact)
│   └── Admin/Games/                # Admin: list, create, edit, delete
├── wwwroot/                        # CSS, JavaScript, video and libraries
├── AdminSetup.cs                   # Grants the admin user its email claim on startup
└── Program.cs                      # Application configuration
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Optional: Visual Studio 2022, VS Code or JetBrains Rider

### Run locally

```bash
# Clone the repository
git clone https://github.com/gen-la/Cybergames.git
cd Cybergames

# Restore dependencies
dotnet restore

# Create or update the SQLite database
dotnet ef database update

# Start the app
dotnet run
```

The app is then available at `https://localhost:7220` or `http://localhost:5067` (see `Properties/launchSettings.json`).

If `dotnet ef` is not installed, run `dotnet tool install --global dotnet-ef` first.

### Configuration

The database connection string is set in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=local.db"
}
```

## 👤 Admin Access

Admin rights are tied to the email address `admin@admin.com`:

1. Register a user with the email `admin@admin.com` on the registration page.
2. Restart the application. On startup, `AdminSetup.cs` gives that user the email claim required by the `AdminOnly` policy.
3. Log in to see the **Admin** link and manage games.

> ⚠️ This setup is meant for development and demo purposes. Before any real deployment, replace it with proper role-based authorization and use a strong admin password.

## 🎓 About the Project

Cybergames was built as a degree project. The goal was to design and implement a complete web application in the .NET ecosystem, combining a database, authentication and authorization, server-side state and a distinctive front-end design.

## 🔮 Possible Future Improvements

- Real payment integration
- Role-based authorization instead of an email claim
- Search and filtering for customers
- Reviews and wishlists
- Automated tests and CI

## 📄 License

Created for educational purposes.

## 🙋 Author

**gen-la** – [GitHub profile](https://github.com/gen-la)
**ma224ip** – [GitHub profile](https://github.com/ma224ip)
