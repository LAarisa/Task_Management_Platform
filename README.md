# 🧩 Multi-Tenant Task Management Platform

A secure, multi-tenant task management API built with **ASP.NET Core**, **Entity Framework Core**, and **JWT Authentication**.  
Includes role-based access control, user and task management, and powerful filtering using LINQ.

---

## 🚀 Features

- ✅ Multi-tenant architecture (isolated data per tenant)
- ✅ Role-based authentication (Admin / User / Manager)
- ✅ Secure password storage with BCrypt
- ✅ JWT-based login and authorization
- ✅ LINQ-powered task filtering & searching
- ✅ Full CRUD for Tasks and Users (Admin-only)
- ✅ Swagger UI for API testing
- ✅ EF Core with Migrations and SQL Server
- ✅ DTOs to separate data contracts

---

## 🧰 Technologies

- ASP.NET Core 8
- Entity Framework Core
- JWT Authentication
- BCrypt.Net (password hashing)
- Swagger / Swashbuckle
- SQL Server (LocalDB)
- LINQ

---

## 🧪 Getting Started

### 📦 Prerequisites

- Visual Studio 2022+ (with ASP.NET + EF workloads)
- .NET 8 SDK
- SQL Server Express / LocalDB

### 🔧 Setup

1. **Clone the repository:**

```bash
git clone https://github.com/yourusername/task-management-platform.git
cd task-management-platform
