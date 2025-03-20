# AuthenticationService

A robust .NET 8 application designed for authentication from multiple sources, built with Clean Architecture using the CQRS pattern with MediatR for request handling and separation of concerns. The service also offers advanced logging via background services, dynamic query building, custom validation logic with FluentValidation, custom model binding and standardized paginated API responses.

Different filters are implemented for access control and ensuring data integrity, as well as for enforcing various authorization policies and role-based access control.

Apart from standard registration and login, it supports Google and Microsoft OAuth login. Users logged in through Google or Microsoft can then use the register endpoint to register in the app and subsequently log in using the standard login with a username and password.

It supports 2FA with OTP, storing OTPs in Redis. Redis is installed in Docker for running on Windows (see instructions below in the "How to Run" section). The authentication response returns a refresh token along with an access token, and the service supports exchanging refresh tokens for new access tokens.

The service dynamically resolves specific services for sending emails such as welcome emails, registration confirmation, password reset, and 2FA OTP using the factory pattern. Emails are sent asynchronously using a custom-built queue. RazorLightEngine is used to dynamically render HTML for emails from a model.

The repository pattern and unit of work pattern are used for accessing repositories, while services handle business logic and manage certain infrastructure-level operations.

---

## Features

### **Custom Validation and Localization**
- **Resource-Based Localization**:
  - Fully localized error messages.
  - Dynamic culture resolution using cookies, query parameters, and browser headers.
- **Custom Validation Logic using FluentValidation**:
  - Ensures proper validation of request models.
  - Provides custom validation error responses.

### **Clean Architecture**
- **Separation of Concerns**:
  - **API Layer**: Handles HTTP requests and responses.
  - **Application Layer**: Contains business logic and use cases.
  - **Domain Layer**: Defines core entities and rules.
  - **Infrastructure Layer**: Manages database access, external services, and background tasks.
  - **Shared Library**: Common utilities, helpers, extensions, resource files, email templates, interfaces and DTOs shared across layers.

### **Advanced Logging**
- **Custom Logger**:
  - Logs are managed through a queue-based **LogBackgroundService** to avoid blocking application performance.
  - Fine-grained control over logging levels for both application and third-party libraries.

### **Robust API Design**
- Bearer token authentication using JWT.
- Standardized API responses for all HTTP status codes, including:
  - `401 Unauthorized`
  - `403 Forbidden`
  - `404 Not Found`
  - `500 Internal Server Error`
- Comprehensive error handling with custom middleware for exception management.

### **Dynamic Query Building**
- Custom query profiles allow dynamic filtering and predicate building for database queries.

### **Action Filters and Authorization Handlers**
- **Custom Action Filters**:
  - Verify the existence of entities before proceeding with operations.
  - Trim string properties in action arguments to remove leading and trailing whitespace.
- **Authorization Handlers**:
  - Enforce access control based on entity ownership or specific access rules.
  - Validate whether the requesting user has the required permissions to access or manipulate the entity identified by its ID.

### **Middleware for Exception Handling and User Context Injection**
- Intercepts exceptions and provides standardized error responses.
- Adds authentication data from the user to the request model dynamically.

### **Behavior Handlers in MediatR**
- Custom behaviors for exception handling and validation, ensuring clean and maintainable CQRS workflows.

---

## Technologies Used

### **Core Frameworks and Libraries**
- **.NET 8**
- **Entity Framework Core**: ORM for database interactions.
- **FluentValidation**: Validation of models using a fluent API.
- **MediatR**: Implementation of the CQRS pattern for handling requests and responses.
- **AutoMapper**: Simplifies object-to-object mapping for DTOs and domain models.
- **RazorLight**: Used to dynamically render HTML templates for emails.
- **Redis Cache**: Stores OTP passwords for 2FA.
- **Docker**: Runs the Redis instance in a containerized environment.

### **Validation and Localization**
- Resource-based localization using `.resx` files.
- Custom validation using FluentValidation.

### **Logging and Monitoring**
- Custom logging system with queued background processing.

---

## Project Structure

### **API Layer**
- Handles HTTP requests and responses.
- Includes controllers for authentication, user management, and other services.

### **Application Layer**
- Business logic and request validation.
- Contains the Features and Behaviors for CQRS commands and queries (using MediatR).
- Contains dynamic mapping profiles for mapping DTOs to entities.

### **Domain Layer**
- Core entities and business rules.
- Defines shared interfaces and contracts.

### **Infrastructure Layer**
- Handles data persistence, background services, and third-party integrations.
- Implements repository pattern and unit of work for database interactions.

### **Shared**
- Contains interfaces used by both the Application Layer and Infrastructure Layer, ensuring Clean Architecture.
- Contains DTOs shared across both layers.
- Contains extension methods and helpers for common utilities.
- Contains resource files for multi-language support.
- Contains email templates used for dynamic email rendering.

---

## How to Run

### **Prerequisites**
- Install **.NET 8 SDK**:  
  Download and install from the official Microsoft website:  
  [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
  
- Configure the **database connection string** in `appsettings.json`.

- Install **Docker**:
  1. Download and install **Docker Desktop** from:  
     [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)
  2. After installation, **restart your system**.
  3. Open **Docker Desktop** and ensure it's running.

- **Run Redis in Docker**:
  1. Open a terminal or PowerShell and pull the Redis image:
     ```bash
     docker pull redis
     ```
  2. Run Redis as a **detached daemon** on port `6379`:
     ```bash
     docker run --name redis-container -d -p 6379:6379 redis
     ```
  3. Verify Redis is running:
     ```bash
     docker ps
     ```
  4. To test Redis connectivity, access the Redis CLI:
     ```bash
     docker exec -it redis-container redis-cli
     ```
     Once inside the Redis CLI, you can run:
     ```bash
     PING
     ```
     If Redis is running correctly, it should return:
     ```
     PONG
     ```

If you encounter permission errors, try running **Docker as Administrator**.

### **Steps**
1. Clone the repository:
   ```bash
   git clone https://github.com/ConradArq/AuthenticationService.git

---

## License
This project is licensed under the [MIT License](LICENSE).