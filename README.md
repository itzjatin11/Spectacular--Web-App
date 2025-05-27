# 🌟 SpecTacular App – E-Commerce Web Application (ASP.NET Core MVC)

**SpecTacular** is a full-featured web application developed using **ASP.NET Core MVC** and **SQL Server**, designed to deliver a seamless online shopping experience for customers and a powerful admin management panel for administrators. It demonstrates a clean architecture, efficient backend management, and a responsive, user-friendly interface.

---

## 🚀 Features

### 👤 Admin Panel

- **User Management**
  - Add, edit, and delete users
  - Display users in an interactive admin dashboard
- **Product Management**
  - Add, edit, and delete products
  - Upload and manage product images with validation
- **Dashboard**
  - Admin dashboard with tabular views for users and products
  - Clean Razor UI (`dashboard.cshtml`) with action buttons and alerts
- **Authentication**
  - Secure admin login with role-based access
  - Session-based login/logout functionality

---

### 🛍️ Customer Shopping Interface

- **Browse Products**
  - Customers can browse available products with images, prices, and categories
- **Shopping Cart**
  - Add products to cart, update quantity, or remove items
  - Real-time total calculation
- **Product Detail View**
  - View detailed product information, including enlarged images and description
- **Responsive Design**
  - Fully responsive layout using Bootstrap

---

## 🧰 Tech Stack

| Layer         | Technology                     |
|---------------|--------------------------------|
| Frontend      | Razor Views, HTML, CSS, Bootstrap |
| Backend       | ASP.NET Core MVC (C#)          |
| Database      | SQL Server                     |
| ORM           | Entity Framework Core          |
| Version Control | Git & GitHub                 |

---

## 🏗️ Architecture

- 3-Tier Architecture (Presentation, Business Logic, Data Access)
- Folder-based organization for Controllers, Models, Views, and Services
- Strong separation of concerns for maintainability and scalability

---

## 🛠️ Getting Started

### Prerequisites

- [.NET SDK 6+](https://dotnet.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or VS Code

### Setup Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/spectacular-app.git

2. Open the solution file in Visual Studio.

3. Configure your database connection string in appsettings.json.

4. Apply migrations or update the database (if using EF Core):
   dotnet ef database update

5. Run the app:
   dotnet run

