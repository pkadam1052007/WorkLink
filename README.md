# WorkLink

[![license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE) [![build](https://img.shields.io/badge/build-passing-brightgreen.svg)]() [![issues](https://img.shields.io/badge/issues-welcome-orange.svg)]()

**WorkLink** — a smart worker hiring platform built with ASP.NET Core MVC to connect users with trusted local service professionals (plumbers, electricians, maids, carpenters, painters, and more).

Short summary: find, book, and manage local workers quickly and securely.

Demo: (add link to live demo or demo video)

Quick links:
- Repository: https://github.com/pkadam1052007/WorkLink
- Issues: (link)

---

Badges

Place CI, coverage, license and latest-release badges here. Example:

[![license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE) [![build](https://img.shields.io/badge/build-passing-brightgreen.svg)]() [![release](https://img.shields.io/badge/release-v1.0-blue.svg)]()

---

## Overview

WorkLink helps customers search for workers by service, location, and availability. Workers create profiles, manage availability and bookings; admins monitor activity and manage roles.

Focus areas:
- Real-world service booking workflow
- Role-based authentication
- Worker profile management
- Distance-based booking and service area logic
- Secure ASP.NET Core architecture with responsive UI

---

## Features

User Features

- Register / login
- Search workers by category and location
- View worker profiles and availability
- Book workers and check booking status
- View service history
- Responsive UI for mobile and desktop

Worker Features

- Register / login
- Create and manage professional profile
- Add skills, experience, and service categories
- Accept/decline bookings and manage availability
- Dashboard and booking history

Admin Features

- Manage users and workers
- Monitor and manage bookings
- Role and DB management

---

## Tech Stack

- Frontend: HTML, CSS, Bootstrap, Razor Views
- Backend: ASP.NET Core MVC
- Database: SQL Server + Entity Framework Core
- Authentication: ASP.NET Identity
- Language: C# (.NET 6/7 — specify exact version used)
- IDE: Visual Studio

Add a versions table or a dockerfile for reproducible environments.

---

## Project Structure

```bash
WorkLink/
├── Controllers/
├── Models/
├── Views/
├── Data/
├── wwwroot/
├── Migrations/
├── appsettings.json
├── Program.cs
└── README.md
```

Notes: Controllers contain MVC controllers; Data contains DbContext and migrations; Views use Razor pages.

---

## Installation

Prerequisites

- .NET SDK 6.0/7.0 (match the project SDK)
- SQL Server (or SQL Server Express / LocalDB)
- Optional: Visual Studio 2022 or VS Code

1) Clone the repository

```bash
git clone https://github.com/pkadam1052007/WorkLink.git
cd WorkLink
```

2) Restore packages

```bash
dotnet restore
```

3) Configure database connection

Edit appsettings.json and set DefaultConnection (see Environment Variables section below).

Example (appsettings.json):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=WorkLinkDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

4) Apply EF Core migrations (two common options):

- Using dotnet-ef CLI:

```bash
dotnet tool install --global dotnet-ef # if not installed
dotnet ef database update
```

- Or using Package Manager Console (Visual Studio):

```powershell
Update-Database
```

5) Run the application

- Using dotnet CLI:

```bash
dotnet run
```

- Or open the solution in Visual Studio and press F5 / run.

Troubleshooting

- If connection errors occur, verify SQL Server is running and the connection string is reachable.
- If migrations fail, ensure EF Core tools and the correct target project are selected.

---

Environment Variables / appsettings

Place secrets and environment-specific settings in appsettings.Development.json or use environment variables.

Required:
- ConnectionStrings:DefaultConnection
- (Optional) SMTP or third-party API keys

Example snippet (appsettings.Development.json):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=WorkLinkDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

For production, prefer environment variables or a secrets manager.

---

Run Locally

1. Ensure prerequisites are installed and DB configured.
2. Apply migrations (dotnet ef database update).
3. Run with dotnet run or from Visual Studio.
4. Open browser at https://localhost:5001 or the configured port.

---

Screenshots

(Add screenshots here to showcase the UI — user search, worker profile, booking flow, worker dashboard.)

Example:

<img width="1359" height="622" alt="Screenshot 2026-03-10 001740" src="https://github.com/user-attachments/assets/98a5fb4f-4ed4-49f6-8f4b-c29c2a52afb8" />


---

Usage / Examples

Typical user flow:
1. Register or login as a user.
2. Search for a service (e.g., plumber) and filter by location.
3. View a worker profile and available times.
4. Book a service and check booking status in your dashboard.

API (if any):
- If you expose APIs, list endpoints here (e.g., GET /workers, POST /bookings). Add sample requests and responses.

---

## Main Modules

- Authentication System
- Worker Profile Management
- Service Category Management
- Booking System
- Distance Calculation
- Dashboard Analytics
- Role-based Access

---

Running Tests

Currently no test project is included (or add instructions if tests exist).

If you add tests, run them via:

```bash
dotnet test
```

Add unit tests for services, controllers, and integration tests for booking flows.

---

Contributing

Contributions are welcome. Suggested workflow:

1. Fork the repo.
2. Create a feature branch: git checkout -b feat/your-feature
3. Commit changes: git commit -m "feat: add ..."
4. Push and open a Pull Request.

Guidelines:
- Follow C# naming and style conventions.
- Add unit tests for new functionality.
- Keep PRs small and focused.

Add a CODE_OF_CONDUCT.md and CONTRIBUTING.md files for more details.

---

Roadmap / Future Improvements

Planned features:
- Live worker tracking
- Payment gateway integration
- Worker ratings and reviews
- AI-based worker recommendation
- Chat system and OTP verification
- Android app version

---

## Author

**Prabhav Pramod Kadam**

IT Diploma Student | Future Cyber Security Engineer | ASP.NET Developer

Contact: (add email, LinkedIn or Twitter link)

---

## License

This project is open-source and available under the **MIT License**.

Include a LICENSE file at the repo root with the full MIT license text.

---

Support

If you need help, open an issue in the repository or contact the author directly.

---
