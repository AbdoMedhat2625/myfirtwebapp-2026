[README.md](https://github.com/user-attachments/files/27085969/README.md)
# MateFinderApp

MateFinderApp is a full-stack matchmaking web application built with ASP.NET Core Web API and Angular. It allows users to register, browse member profiles, like other members, manage profile photos, and update their personal information through a protected client-server workflow.

## Tech Stack

- Backend: ASP.NET Core Web API, Entity Framework Core, SQLite, JWT Authentication
- Frontend: Angular 20, TypeScript, Tailwind CSS, DaisyUI
- Media: Cloudinary for profile photo upload and deletion
- Tooling: Angular CLI, .NET SDK, EF Core Migrations, Git

## Features

- User registration and login with JWT-based authentication
- Protected routes and authenticated API access
- Browse members with filtering, sorting, and pagination
- Member profile details and editable profile data
- Like, unlike, and view liked, liked-by, and mutual lists
- Upload, delete, and set a main profile photo
- Centralized exception handling on the API side
- Seeded development data for quick local setup

## Project Structure

```text
MateFinderApp/
|- API/       ASP.NET Core backend
|- client/    Angular frontend
|- MateFinderApp.slnx
```

## Backend Overview

The backend exposes REST endpoints for:

- `account` for register and login
- `members` for listing, viewing, updating members, and managing photos
- `likes` for toggling likes and querying liked relationships
- `messages` for message creation and inbox-style retrieval

It uses Entity Framework Core with SQLite and applies migrations automatically on startup. Seed data is loaded from `API/Data/UserSeedData.json` when the database is empty.

## Frontend Overview

The Angular client includes:

- Authentication flow with persisted current user state
- Member listing with filters and pagination
- Member detail pages with nested routes
- Likes pages with tabbed filtering
- HTTP interceptors for JWT, loading state, and error handling
- Route guards and unsaved changes protection

## Prerequisites

- .NET SDK 10.0 or compatible preview matching `net10.0`
- Node.js 20+
- npm

## Local Setup

### 1. Clone the repository

```bash
git clone https://github.com/AbdoMedhat2625/MateFinder.git
cd MateFinder
```

### 2. Configure the API

The API uses `API/appsettings.Development.json` for local development.

Current local defaults:

- Database: SQLite with `Data Source=dating.db`
- API base URL expected by the client: `https://localhost:5001/api/`

If you want photo uploads to work, add Cloudinary credentials under `CloudinarySettings` in your development configuration:

```json
{
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

### 3. Run the backend

```bash
cd API
dotnet restore
dotnet run
```

The API will apply migrations and seed the database automatically at startup.

### 4. Run the frontend

Open a second terminal:

```bash
cd client
npm install
npm start
```

The Angular app runs on:

- `https://localhost:4200`

## Development Notes

- The Angular dev server is configured with local SSL certificates in `client/ssl/`
- The frontend development environment points to `https://localhost:5001/api/`
- A default seeded password is set in code for development users: `Pa$$w0rd`

## Future Improvements

- Complete the standalone messages page in the Angular client
- Add automated tests for API and UI flows
- Add deployment instructions for production hosting
- Improve validation and user-facing error messages

## Repository

- GitHub: [github.com/AbdoMedhat2625/MateFinder](https://github.com/AbdoMedhat2625/MateFinder)
