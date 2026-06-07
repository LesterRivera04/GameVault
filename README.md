# GameVault

## Overview

GameVault is a video game catalog web application built with ASP.NET Core MVC following Clean Architecture principles. The application allows users to discover, browse, search, and manage video games through a modern and responsive interface powered by external game data.

The solution demonstrates enterprise development practices including Clean Architecture, Service Layer, Repository Pattern, Dependency Injection, DTOs, JWT Authentication, Entity Framework Core, Fluent API, Unit Testing, REST APIs, and Minimal APIs.

GameVault integrates with the RAWG Video Games Database API to provide rich game information including names, genres, platforms, ratings, release dates, and cover images.

---

## Features

### User Management

* Create users.
* Edit user information.
* Delete users.
* View registered users.

### Game Catalog

* Browse a large collection of video games.
* Search games by name.
* View detailed game information.
* Pagination support for improved performance.
* Dynamic game images with fallback placeholders.

### RAWG API Integration

The application consumes the RAWG API to retrieve:

* Game titles.
* Ratings.
* Genres.
* Platforms.
* Release dates.
* Cover images.

This allows GameVault to provide up-to-date information without maintaining a large local database of games.

### Dashboard Analytics

A custom Minimal API provides dashboard metrics including:

* Total games stored.
* Most popular games based on user activity.
* Recently added games.

The dashboard endpoint exposes lightweight statistical information that can be consumed by the MVC application.

### Authentication

The backend API includes JWT Authentication support.

Features include:

* Secure token generation.
* Claims-based authentication.
* Protected API endpoints.
* Authorization-ready architecture.

---

## Architecture

The solution follows Clean Architecture principles:

MVC
↓
MVC Services
↓
REST API Controllers
↓
API Services
↓
Repository
↓
Entity Framework Core
↓
SQL Server

A separate Minimal API project is used to expose dashboard-related statistics.

This architecture promotes:

* Separation of concerns.
* Maintainability.
* Scalability.
* Testability.

---

## Design Patterns

### Repository Pattern

Encapsulates database operations and abstracts data access logic.

### Service Layer

Centralizes business rules and application workflows.

### Dependency Injection

Used throughout the application to improve flexibility, maintainability, and testability.

### DTO Pattern

Separates domain entities from external and presentation-layer models.

---

## Database Design

Current entities include:

* Game
* Genre
* Platform
* GameGenre
* GamePlatform
* UserGame
* Usuario

Entity relationships and constraints are configured using Fluent API.

---

## Technologies

### Backend

* ASP.NET Core MVC
* ASP.NET Core Web API
* ASP.NET Core Minimal APIs
* Entity Framework Core
* Fluent API
* SQL Server
* JWT Authentication

### Frontend

* Razor Views
* Bootstrap 5
* HTML5
* CSS3
* JavaScript

### Testing

* xUnit
* Moq

### API Integration

* RAWG API
* HttpClientFactory
* Custom Minimal API

---

## Solution Structure

### GameVault

ASP.NET Core MVC application responsible for:

* User interface.
* Game catalog.
* Search functionality.
* Dashboard visualization.

### GameVault.API

REST API responsible for:

* Business logic.
* JWT authentication.
* Repository layer.
* Entity Framework Core integration.

### MinimalAPI.Dashboard

Custom Minimal API responsible for:

* Dashboard metrics.
* Popular games statistics.
* Recently added games data.

### GameVault.Tests

Unit tests using xUnit and Moq.

---

## Unit Testing

The project includes unit tests focused on business logic and service layer validation.

Covered scenarios include:

* Creating games.
* Retrieving games.
* Updating games.
* Deleting games.
* Repository interaction verification.
* Service validation logic.

Database dependencies are isolated using mocked repositories.

---

## Future Improvements

* Complete JWT authentication flow in MVC.
* User game collections.
* Favorite games system.
* User reviews and ratings.
* Advanced filtering options.
* Dashboard visual charts.
* CI/CD pipeline integration.
* Cloud deployment.
* Role-based authorization.
* Game recommendation engine.

---

## Screenshots

### Home Page

![Home Page](ImagesREADME/Home%20GameVault.png)

### Games Catalog

Top ↓
![Games Catalog 1](ImagesREADME/Games%201.png)

Bottom ↓
![Games Catalog 2](ImagesREADME/Games%202.png)

---

## What I Learned

This project was developed as part of my learning journey in enterprise-level .NET development and allowed me to gain hands-on experience with:

* Clean Architecture.
* REST API development.
* JWT Authentication.
* Entity Framework Core.
* Fluent API.
* External API consumption.
* Minimal APIs.
* Unit Testing with xUnit and Moq.
* Dependency Injection.
* Repository and Service Layer patterns.
