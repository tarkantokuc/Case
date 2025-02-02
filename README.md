# Case

Stock Tracking System - Backend Development Case

## Tech Stack

- **.NET 8**
- **Entity Framework Core** (with PostgreSQL provider)
- **JWT Token Authentication**
- **Swagger** for API documentation
- **C# 10+**

## Features

- **User Authentication and Authorization**
  - `POST /api/auth/register` - Register a new user.
  - `POST /api/auth/login` - Authenticate a user and return a JWT token.
- **Product Management**
  - `GET /api/products` - Retrieve all products.
  - `POST /api/products` - Add a new product.
  - `PUT /api/products/{id}` - Update an existing product.
- **Supply Management**
  - `POST /api/products/{productId}/supplies` - Add supply data for a specific product.
- **Sale Management**
  - `POST /api/products/{productId}/sales` - Record a sale for a specific product.
- **Transaction Queries**
  - `GET /api/transactions` - Retrieve supply and sale transactions.

## Prerequisites

- .NET 7 SDK or later
- A relational database system (PostgreSQL)
- Visual Studio 2022

## Required NuGet Packages

The following NuGet packages are required for the project:

- **Microsoft.EntityFrameworkCore**
- **Microsoft.EntityFrameworkCore.Design**
- **Microsoft.EntityFrameworkCore.Tools**
- **Npgsql.EntityFrameworkCore.PostgreSQL** (if using PostgreSQL)  
- **Microsoft.AspNetCore.Authentication.JwtBearer**
- **Microsoft.IdentityModel.Tokens**
- **Microsoft.OpenApi.Models**
- **Microsoft.AspNetCore.Identity**
- **BCrypt.Net-Next** (for password hashing)