# Game of Thrones - Item Catalog

A full-stack web application for managing and browsing Game of Thrones items with categories, rarities, and analytics.

## Project Structure

```
GameCatalog/
├── Backend/
│   └── GameCatalog/              # C# ASP.NET Core 10 backend
│       ├── Controllers/          # API endpoints
│       ├── Models/               # Data models (GameItem, GameCategory, Rarity)
│       ├── Services/             # Business logic (GameService, GameAnalytics)
│       ├── Repositories/         # Data access layer
│       ├── Program.cs            # Application configuration
│       └── GameCatalog.csproj    # Project file
├── Frontend/                     # Static web files
│   ├── index.html                # Main HTML page
│   ├── app.js                    # Frontend logic and API calls
│   └── styles.css                # Styling
└── GameCatalog.sln               # Solution file
```

## Features

- **Browse Items** - View all Game of Thrones items in a grid layout
- **Filter & Search** - Filter by category, rarity, or search by name
- **Create Items** - Add new items with name, category, rarity, level requirement, and price
- **Edit Items** - Modify existing item details
- **Delete Items** - Remove items from the catalog
- **Generate Random Items** - Bulk create items with random properties
- **Analytics** - View statistics:
  - Total items count
  - Average price
  - Items by category
  - Items by rarity
  - Highest level item

## Getting Started

### Prerequisites

- .NET 10 SDK
- A modern web browser

### Installation & Running

1. **Navigate to the backend directory:**
   ```powershell
   cd Backend/GameCatalog
   ```

2. **Build the project:**
   ```powershell
   dotnet build
   ```

3. **Run the application:**
   ```powershell
   dotnet run
   ```
   Or run the compiled DLL directly:
   ```powershell
   dotnet bin/Debug/net10.0/GameCatalog.dll
   ```

4. **Open in browser:**
   Navigate to `http://localhost:5000`

## API Endpoints

All endpoints are prefixed with `/api/gameItems`

### Items Management
- `GET /` - Get all items
- `GET /{id}` - Get item by ID
- `POST /` - Create new item
- `PUT /{id}` - Update item
- `DELETE /{id}` - Delete item

### Data Operations
- `GET /categories` - Get all categories
- `GET /rarities` - Get all rarities
- `POST /generate` - Generate random items
- `GET /analytics` - Get analytics data

## API Request Examples

### Create Item
```json
POST /api/gameItems
{
  "name": "Excalibur",
  "category": "Weapon",
  "rarity": "Legendary",
  "levelRequirement": 50,
  "price": 5000.00
}
```

### Generate Random Items
```json
POST /api/gameItems/generate
{
  "count": 10
}
```

## Technologies Used

### Backend
- **Framework:** ASP.NET Core 10
- **Language:** C#
- **Architecture:** Repository Pattern with Services

### Frontend
- **HTML5** - Markup
- **CSS3** - Styling
- **JavaScript (ES6)** - Client-side logic
- **Fetch API** - HTTP requests

### Features
- **CORS** - Enabled for cross-origin requests
- **JSON Serialization** - Enum conversion support
- **Static File Serving** - Frontend served by backend
- **In-Memory Database** - Data persists during session

## Project Features Breakdown

### Models
- `GameItem` - Represents an item with properties like name, category, rarity, level requirement, price, and creation date
- `GameCategory` - Enum for item categories (Weapon, Armor, Artifact, etc.)
- `Rarity` - Enum for item rarities (Common, Uncommon, Rare, Epic, Legendary)

### Services
- `GameService` - Core business logic for CRUD operations and analytics
- `GameAnalytics` - Aggregated statistics about the catalog

### Repositories
- `IGameRepository` - Interface for data access
- `InMemoryGameRepository` - Implementation using in-memory storage

## User Interface

The web interface includes:
- **All Items Tab** - Browse and manage all items
- **Create Item Tab** - Form to add new items
- **Analytics Tab** - View catalog statistics
- **Search & Filters** - Filter by category, rarity, or search by name
- **Modal Dialogs** - For editing items
- **Responsive Grid Layout** - Clean display of items with details

## Development Notes

- The frontend files are served by the backend from the `Frontend/` folder
- API calls use relative URLs (`/api/gameItems`) for seamless integration
- CORS policy allows requests from any origin for flexibility
- The in-memory repository means data is reset when the server restarts

