# FreshCart: Online Grocery Store
FreshCart is a modern, scalable, and secure grocery store web application designed using the latest technologies. It integrates Angular (Frontend) and ASP.NET Core (Backend) to deliver a seamless user experience, alongside robust security measures and efficient data handling. This project provides a rich feature set for users, admins, and anonymous visitors, ensuring smooth grocery shopping and administrative operations.

#### Landing Page
![shopfreshcart netlify app_](https://github.com/user-attachments/assets/0f783eee-2cc0-40c4-9170-b2c9aa0c58a6)


# Technologies
## Front-End (Angular 15)
- Angular: Utilized as the front-end framework, leveraging the latest version (Angular 15) to build a highly interactive and responsive user interface.
- Bootstrap: Used to enhance the UI with pre-styled, responsive components for a polished look and feel across devices.
- Reactive Forms: Provides dynamic form handling, allowing real-time validation and improved user interactions.
- RxJS: Utilized to manage asynchronous operations and handle data streams, ensuring a responsive experience for users.
- Toastr: A highly customizable notification system to provide real-time feedback to users for actions like successful product additions or error messages.
- AOS (Animate On Scroll): Utilized for adding smooth and visually appealing scroll animations to enhance the user experience as users scroll through product listings and pages.

## Back-End (ASP.NET Core 5)
- ASP.NET Web API: The back-end is built using ASP.NET Core, offering a scalable and robust server-side infrastructure for managing business logic, data, and API endpoints.
- Entity Framework Core: The application leverages Entity Framework Core for ORM-based data access, simplifying CRUD operations, and providing powerful querying capabilities while managing interactions with SQL Server.
- JWT Token Authentication: Implements secure user authentication using JWT tokens, ensuring protected access to various resources within the app.
- Role-Based Authorization: Integrates role-based access control to ensure secure and tailored user access to different parts of the application.

## Database
FreshCart utilizes SQL Server for its relational data storage. 
Entity Framework Core simplifies database operations by providing an ORM-based approach to interact with the SQL Server database, abstracting raw SQL queries into more manageable code with LINQ support.

## Design Patterns
The application utilizes SOLID design principles and several key design patterns to ensure scalability and maintainability:
- Dependency Injection: Promotes loose coupling and enhances testability by injecting dependencies into classes.
- Repository Pattern: Provides an abstraction layer for database access, ensuring separation between business logic and data access.
- Singleton: Ensures that certain services, like HTTP services, are instantiated only once throughout the application’s lifecycle.
- Decorator: Used for extending and adding functionality to objects without modifying their structure.

## Deployment
FreshCart is deployed using various Azure services and external platforms:
- Azure SQL Database: The application’s relational data is stored in a fully-managed SQL Server database on Azure.
- Azure App Services: The back-end API is deployed on Azure App Services for secure, scalable hosting.
- Azure Storage (Blob): Images and static assets are stored and served from Azure Blob Storage.
- Netlify: The Angular frontend is deployed on Netlify for fast, static site hosting with automatic CI/CD integration.

# Setup Instructions

## Back-End (ASP.NET Core)
1. Clone the repository and navigate to GroceryStoreBackEnd.
2. Update the connection string in appsettings.json to point to your local SQL Server.
3. Open the solution in Visual Studio and set GroceryStoreBackEnd as the startup project.
4. Open the NuGet Package Manager Console and run the following commands to create the database:
```
Add-Migration <NameOfMigration>
Update-Database
```
5. Launch the backend API using your desired configuration (HTTPS or IIS).

## Front-End (Angular)
1. Navigate to GroceryStoreFrontEnd.
2. Install the required dependencies:
``` npm install ```
3. Start the Angular server:
``` ng serve ```
4. Visit http://localhost:4200 in your browser to see the app in action.


