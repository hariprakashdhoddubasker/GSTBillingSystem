# GSTBillingSystem – GST Billing & Invoicing System

## Overview
GSTBillingSystem is a WPF-based desktop billing application tailored for businesses that need to generate GST-compliant invoices (for example, a jewelry or retail shop). Developed in C# .NET, the application simplifies the process of creating professional invoices that adhere to India’s Goods and Services Tax regulations. Users can maintain a catalog of products and customers, generate invoices with automatic tax calculations, and keep track of sales records in an organized manner.

## Features

- **Product & Inventory Management:** Maintain a list of products/items with details like description, price, HSN code, applicable tax rate, and current stock. The system updates stock levels after each sale and can alert users when inventory is low.
- **Customer Management:** Store customer information (name, contact details, and GSTIN if applicable) for quick selection during billing. This helps in maintaining a client database for repeat business and record-keeping.
- **Invoice Generation:** Create invoices with multiple line items selected from the product list. The application automatically calculates GST (splitting into CGST/SGST or IGST based on the customer’s state) for each item and for the invoice total. It supports multiple tax slabs and formats the invoice in a professional layout with all required fields (invoice number, date, taxes, totals, etc.).
- **Printing and PDF Export:** Print invoices directly from the application or export them as PDF files for digital records or email. The invoice layout is designed to meet GST invoice standards, including business branding (logo, address) and tax breakdowns.
- **Sales Reports & History:** View past invoices and generate sales reports over a selected date range. These reports include daily/monthly sales totals and tax summaries, assisting business owners in financial analysis and GST return filing.

## Technical Highlights

- **MVVM Architecture:** GSTBillingSystem follows an MVVM architecture in its WPF implementation, ensuring a clear separation of UI and logic.
- **Data Binding:** Real-time calculations (such as tax and totals) are automatically updated as items are added to invoices.
- **Local Database:** A local SQL-based database (e.g., SQL Server Express or SQLite) stores all master and transaction data for persistence and reliability.
- **ORM Support:** Uses Entity Framework (or ADO.NET) to manage database interactions with consistency and robustness.
- **Reusable Components:** Includes services for tax calculation, repositories for CRUD operations, and shared utilities (possibly from **CommonAppBase**) for tasks like printing and formatting.
- **Productivity-Focused UI:** Intuitive interfaces like drop-down selectors and invoice previews minimize errors and user training time.

## Setup & Installation

### 1. Prerequisites
- Install the appropriate .NET runtime (e.g., .NET Framework 4.x) on your Windows PC.
- Install a local database server (SQL Server Express or SQLite, as supported).

### 2. Source Code
- Clone or download the [GSTBillingSystem](https://github.com/hariprakashdhoddubasker/GSTBillingSystem) repository.
- Navigate to the `GSTBillingSystem\WpfApp` project folder and open the solution file (`.sln`) in Visual Studio.
- Ensure the companion `CommonAppBase` project is included and correctly referenced in the solution.

### 3. Database Setup
- Run the SQL scripts (if provided) to initialize database tables for Products, Customers, Invoices, etc.
- Alternatively, if the app uses Code First migrations, update the connection string in the configuration file and run the app to automatically create the schema.

### 4. Build Application
- Restore required NuGet packages.
- Build the project in Visual Studio, resolving all references and dependencies.

### 5. Run & Usage
- Launch the GSTBillingSystem application.
- Populate the **Master Data** section by adding some products and customers.
- Go to the **Billing** section to create a new invoice:
  - Select a customer.
  - Add product line items.
  - The tax and total are calculated automatically.
  - Save, print, or export the invoice.
- Use the **Reports** section to generate sales summaries or reprint past invoices.

---

**Note:** Make regular backups of your database to prevent data loss and ensure compliance with GST regulations.
