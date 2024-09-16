# Customer Management Application
This is a self-contained Customer Management application built using **ASP.NET Web Forms** as frontend and **ASP.NET Web API** for the backend. It allows to perform CRUD operations (Create, Read, Update, Delete) on customer data.

The backend uses a SQL Server database for storing customer information. The solution also implements proper **API validation** and **separation** between the UI and data layers.

## Features

- Add, edit, and delete customer records.
- ASP.NET Web Forms as the UI for customer management.
- ASP.NET Web API for backend data access and API endpoints.
- SQL Server integration for customer data storage.
- Input validation (e.g., Email, Phone Number) in both the API and frontend.
- API separation with clean architecture.

## Technologies Used

- **Frontend**: ASP.NET Web Forms
- **Backend**: ASP.NET Web API
- **Database**: SQL Server 

## Prerequisites

Before you begin, ensure you have met the following requirements:

- Visual Studio 2019/2022 or later
- .NET Framework 4.7.2 or later
- SQL Server Express
- Chrome / Microsoft Edge browser

## Setup Instructions

### 1. Clone the Repository

Clone this repository to your local machine using:

git clone https://github.com/DeepaRajendran1006/CustomerManagement_Pinewood

### 2. Open the solution

Open the solution in Visual Studio

### 3. Database Setup

- The repository contains the SQL file [DBScript.sql] under 'db' folder which creates the Customer Database and a respective table to store the customer information.

- Update the connection string in the Web.config of the Web API project to connect to the SQL Server instance:

 <connectionStrings>
    <add name="DefaultConnection" connectionString="Server=<server-name>;Initial Catalog=CustomerDB;Integrated Security=True" providerName="System.Data.SqlClient"/>
  </connectionStrings>

### 4. Run the solution

- Press F5 to run the solution.
- Both the API and WebForms are set as startup projects.
- Browser opens both the API and Customer Information forms
- API can be verified by navigating to https://localhost:44348/api/Customer
- WebForms loaded the page with Customer Information page to add, edit, or delete customer records

### 5. Testing the API

To manually test the API endpoints, here are the available routes:

- **GET** `/api/Customer`: Retrieves a list of all customers.
- **GET** `/api/Customer/{id}`: Retrieves a customer by ID.
- **POST** `/api/Customer`: Adds a new customer.
- **PUT** `/api/Customer/{id}`: Updates an existing customer.
- **DELETE** `/api/Customer/{id}`: Deletes a customer by ID.

### 6. API Validation and Separation

- **Frontend Validation**: Client-side validation for fields like Email and Phone Number in the **Web Forms** project using `RequiredFieldValidator`, `RegularExpressionValidator`, etc.
- **API Validation**: Server-side validation for customer data in the **Web API** project to ensure valid email format, phone number length, and required fields.

## How to Use the Application

1. Open the `Customer Management` form in the **Web Forms** project.
2. A table will be displayed with the list of customers.
3. Use the form to **Add**, **Edit**, or **Delete** customers.
4. The changes are reflected in the SQL Server database (if connected).

## Future Improvements

- Can add user authentication for securing the application.
- Can implement pagination in the customer list.
- Can add search functionality for filtering customers.
- Can enhance the frontend UI with more advanced CSS or JavaScript frameworks.
