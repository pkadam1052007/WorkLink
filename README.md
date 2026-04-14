# WorkLink

<p align="center">
  <img src="./Screenshot 2026-01-23 005305.png" alt="WorkLink Logo" width="180"/>
</p>

**WorkLink** is a smart worker hiring platform built with **ASP.NET Core MVC** that connects users with trusted local service professionals such as **plumbers, electricians, maids, carpenters, painters, and other skilled workers**.

The platform is designed to make **finding, booking, and managing local workers simple, fast, and reliable**.

---

## Overview

WorkLink helps customers easily search for workers based on their required service, location, and availability.

Workers can create professional profiles, showcase their skills, set service areas, and manage bookings through their dashboard.

This project focuses on:

* Real-world service booking workflow
* Role-based authentication
* Worker profile management
* Distance-based booking logic
* Secure ASP.NET Core architecture
* Responsive UI for all devices

---

## Features

### User Features

* User registration and login
* Search workers by category
* View worker profiles
* Book workers instantly
* Check booking status
* View service history
* Responsive mobile-friendly UI

### Worker Features

* Worker registration and login
* Create professional profile
* Add skills and experience
* Set service category
* Accept or reject bookings
* Worker dashboard
* Manage availability
* Booking history tracking

### Admin Features

* Manage users and workers
* Monitor bookings
* View platform activity
* Role management
* Database control

---

## Tech Stack

* **Frontend:** HTML, CSS, Bootstrap, Razor Views
* **Backend:** ASP.NET Core MVC
* **Database:** SQL Server + Entity Framework Core
* **Authentication:** ASP.NET Identity
* **Language:** C#
* **IDE:** Visual Studio

---

## Project Structure

```bash
WorkLink/
│── Controllers/
│── Models/
│── Views/
│── Data/
│── wwwroot/
│── Migrations/
│── appsettings.json
│── Program.cs
│── README.md
```

---

## Installation

### 1) Clone Repository

```bash
git clone https://github.com/pkadam1052007/WorkLink
cd WorkLink
```

### 2) Setup Database

Update your **SQL Server connection string** inside:

```json
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=WorkLinkDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3) Apply Migrations

```bash
Update-Database
```

### 4) Run Project

```bash
dotnet run
```

---

## Main Modules

* Authentication System
* Worker Profile Management
* Service Category Management
* Booking System
* Distance Calculation
* Dashboard Analytics
* Role-based Access

---

## Use Cases

This platform is useful for hiring:

* Plumbers
* Electricians
* Maids
* Carpenters
* Painters
* Mechanics
* Cleaners
* Technicians

---

## Security Highlights

* ASP.NET Identity authentication
* Role-based authorization
* Secure database access with EF Core
* Input validation
* Protected worker routes
* User session management

---

## Future Improvements

* Live worker tracking
* Payment gateway integration
* Worker ratings and reviews
* AI-based worker recommendation
* Chat system
* OTP verification
* Android app version

---

## Why This Project?

WorkLink solves a real-world problem by making it easier for people to hire trusted local workers without searching offline.

It combines:

* software engineering
* database management
* authentication
* clean UI design
* scalable architecture

making it a strong **portfolio project for internships, placements, and GitHub showcase**.

---

## Author

**Prabhav Pramod Kadam**

IT Diploma Student | Future Cyber Security Engineer | ASP.NET Developer

---

## License

This project is open-source and available under the **MIT License**.
