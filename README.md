# FreshCart

A grocery store webapp built using Angular as the front-end framework and ASP.NET Core as the back-end framework.

## Technologies

### Front-End (Angular 15)

- **Bootstrap**: The application leverages the Bootstrap to enhance the UI with pre-styled and responsive components.
- **Reactive Forms**: Angular's reactive forms approach is utilized for form handling and validation, providing a dynamic and interactive user interface.
- **RxJS**: Reactive Extensions for JavaScript (RxJS) is employed to handle asynchronous operations and manage data streams within the application.

### Back-End (ASP.NET Core 5)

- **ASP.NET Web API**: The back-end of the grocery store application is developed using the ASP.NET Web API framework, providing a robust and scalable server-side infrastructure.
- **JWT Token Authentication**: The application uses JWT (JSON Web Token) authentication to secure the APIs and ensure authorized access to protected resources.
- **Role Based Authorization**: Implemented role based authorization so that the APIs are secure from any unauthorized access.

### Backend Structure

- **Business Layer**: Implements the business logic and rules of an application. It acts as a bridge between the user interface and the data access layer.
- **Data Access Layer**: Responsible for handling data interaction with the storage system. It provides methods for CRUD operations, abstracts the underlying data storage implementation, handles querying and filtering, manages transactions, optimizes performance, and incorporates security measures.
- **Presentation Layer (.NET core web API)**: Defines all the API endpoints.
- **Web Layer**: Handles the user interface and user interactions. It focuses on designing and rendering the user interface components and handling user input events.

### Design Patterns Used

- Dependency Injection
- Repository Pattern
- Singleton
- Decorator

### DataBase (SQL Server)

The grocery store project makes use of SQL Server as the chosen database management system for storing and managing data associated with products, orders, users, and other pertinent information. SQL Server is a powerful and widely employed relational database management system (RDBMS) renowned for its robust data storage, transactional capabilities, and efficient data retrieval capabilities.

#### Tables

- Products
- Orders
- Users
- Cart
- Reviews
- Roles
- Categories

## Role Based Access:

### Anonymous user has access to:

- Dashboard
- Product page
- Login
- Register

### Logged in user has access to:

- Dashboard
- Product page
- Add Reviews
- Cart
- My Orders
- Place order
- Sign out

### Admin has access to:

- Add product
- Delete Product
- Edit Product
- Add reviews
- Top 5 most ordered products in a month
- Sign out


## SetUp

### Prerequisites

- Node.js: Install Node.js latest version
- Angular CLI: Install the Angular CLI globally using the following command:

```
npm install -g @angular/cli
```

Clone the repository and do the following to run the app:

### Front-End (Angular)

1. Navigate to GroceryStoreFrontEnd
2. Install dependencies

```
npm install
```

3. Start server

```
ng serve
```

4. Visit http://localhost:4200

### Back-End (ASP.NET Core)

Required: latest version of Visual Studio 2022

### Setting Up Back-end

- Inside appsettings.json change the server name in the connection string to your local SQL server string.

- Open solution in Visual Studio 2022 > Go to Tools > NuGet Package Manager > Package Manager Console.
- Select **GroceryStore.DAL\GroceryStore.DataAccessLayer** in default project option shown above the package manager console.
- Make the project **GroceryStoreBackEnd** as the startup project by right clicking on it and selecting _Set as startup project_ option.
- After above steps use the following commands one by one to add migration and create database inside your sql server.

```
  Add-Migration <NameOfMigration>
  Update-Database
```

After the Back end is setup, you can launch the server using any of the launch configuration like https or IIS by selecting it toolbar.
