# YouTube Clone

A full-stack YouTube clone application built with .NET 9, Blazor WebAssembly, and Entity Framework Core.

## Features

- **User Authentication**: Register, login, and JWT-based authentication
- **Video Management**: Upload, search, and view videos
- **YouTube API Integration**: Search and display trending videos from YouTube
- **User Interactions**: Like videos, comment on videos, subscribe to channels
- **Responsive Design**: Mobile-friendly interface with Bootstrap
- **Cloud Storage**: Azure Blob Storage for video files
- **Real-time Features**: Comments and interactions

## Architecture

The application follows a clean architecture pattern with the following projects:

- **YouTubeClone.Client**: Blazor WebAssembly frontend
- **YouTubeClone.Server**: ASP.NET Core Web API backend
- **YouTubeClone.Shared**: Shared models and DTOs
- **YouTubeClone.Infrastructure**: Data access, services, and external integrations

## Technology Stack

### Backend
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Azure Blob Storage
- YouTube Data API v3

### Frontend
- Blazor WebAssembly
- Bootstrap 5
- Bootstrap Icons
- Blazored LocalStorage

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (LocalDB or full instance)
- Azure Storage Account (optional, uses development storage by default)
- YouTube Data API Key (optional, uses default key)

### Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd YouTubeClone
   ```

2. **Configure the database**
   - Update the connection string in `src/YouTubeClone.Server/appsettings.json`
   - The application will create the database automatically on first run

3. **Configure external services (optional)**
   - Update `appsettings.json` with your own API keys:
     - YouTube Data API key
     - Azure Blob Storage connection string
     - Google/Microsoft OAuth credentials

4. **Run the application**
   ```bash
   # Start the API server
   cd src/YouTubeClone.Server
   dotnet run

   # Start the Blazor client (in a new terminal)
   cd src/YouTubeClone.Client
   dotnet run
   ```

5. **Access the application**
   - Client: https://localhost:7001
   - API: https://localhost:7000
   - Swagger UI: https://localhost:7000/swagger

## Configuration

### Database
The application uses SQL Server with Entity Framework Core. The connection string can be configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=YouTubeCloneDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### External Services

#### YouTube API
```json
{
  "YouTubeApi": {
    "ApiKey": "your-youtube-api-key"
  }
}
```

#### Azure Blob Storage
```json
{
  "ConnectionStrings": {
    "BlobStorage": "your-azure-storage-connection-string"
  }
}
```

#### Authentication Providers
```json
{
  "Authentication": {
    "Google": {
      "ClientId": "your-google-client-id",
      "ClientSecret": "your-google-client-secret"
    },
    "Microsoft": {
      "ClientId": "your-microsoft-client-id",
      "ClientSecret": "your-microsoft-client-secret"
    }
  }
}
```

## Features Overview

### User Management
- User registration and login
- JWT-based authentication
- Profile management
- Channel creation

### Video Features
- Video upload with multiple quality processing
- Video search using YouTube API
- Trending videos display
- Video metadata management

### Social Features
- Like/dislike videos and comments
- Comment on videos
- Subscribe to channels
- User playlists

### Admin Features
- User management
- Content moderation
- Analytics dashboard

## API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login

### Videos
- `GET /api/video/search` - Search videos
- `GET /api/video/trending` - Get trending videos
- `POST /api/video/upload` - Upload video
- `GET /api/video/{id}/comments` - Get video comments
- `POST /api/video/{id}/comments` - Add comment
- `POST /api/video/{id}/like` - Like/unlike video

### User
- `POST /api/user/subscribe` - Subscribe/unsubscribe
- `POST /api/user/comments/{id}/like` - Like/unlike comment
- `PUT /api/user/comments/{id}` - Update comment
- `DELETE /api/user/comments/{id}` - Delete comment

## Development

### Project Structure
```
src/
├── YouTubeClone.Client/          # Blazor WebAssembly app
│   ├── Pages/                    # Razor pages
│   ├── Shared/                   # Shared components
│   ├── Services/                 # Client services
│   └── wwwroot/                  # Static files
├── YouTubeClone.Server/          # ASP.NET Core API
│   ├── Controllers/              # API controllers
│   └── Program.cs                # Application entry point
├── YouTubeClone.Shared/          # Shared models and DTOs
│   ├── Models/                   # Domain models
│   └── DTOs/                     # Data transfer objects
└── YouTubeClone.Infrastructure/  # Data and services
    ├── Data/                     # Entity Framework context
    ├── Identity/                 # Identity models
    └── Services/                 # Business services
```

### Adding New Features

1. **Add models** in `YouTubeClone.Shared/Models/`
2. **Add DTOs** in `YouTubeClone.Shared/DTOs/`
3. **Update DbContext** in `YouTubeClone.Infrastructure/Data/`
4. **Add services** in `YouTubeClone.Infrastructure/Services/`
5. **Add controllers** in `YouTubeClone.Server/Controllers/`
6. **Add pages/components** in `YouTubeClone.Client/`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- YouTube Data API for video content
- Bootstrap for UI components
- Entity Framework Core for data access
- Blazor for the frontend framework
