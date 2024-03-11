# SimpleSales

SimpleSales is a .NET Core MVC API project designed to provide backend functionality for a sales application. This project follows a structure where the backend code resides in the `backend` folder and the frontend code resides in the `frontend` folder.

## Project Structure

- `backend/`: Contains the backend code for the API developed using .NET Core MVC.
- `frontend/`: Holds the frontend code of the application. This could be a separate application using frameworks like React, Angular, or Vue.js.

## Backend

The backend of SimpleSales is built using .NET Core MVC. It provides RESTful APIs to handle various functionalities required for the sales application.

## Frontend

The frontend of SimpleSales is located in the `frontend` folder. It's responsible for providing a user interface to interact with the backend APIs. You can use any frontend framework or technology of your choice to build the frontend application.

## Installation and Setup

### Prerequisites

- [.NET SDK 7](https://dotnet.microsoft.com/download/dotnet/7.0) must be installed.
- [Node.js](https://nodejs.org/en/) should be installed if you're working with a frontend framework that requires it.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/Fariszz/SimpleSales.git
   ```

2. Navigate to the backend directory:
   
   ```bash
   cd SimpleSales/backend
   ```

3. Restore dependencies and build the backend
   
   ```bash
   dotnet restore
   dotnet build
   ```

3. Run the backend
   
   ```bash
   dotnet run
   ```
