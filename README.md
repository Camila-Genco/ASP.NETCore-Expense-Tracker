# Expense Tracker 💼📊💸
> An app build with ASP.NET Core MVC

This is an app crafted for you to track your income, manage your expenses and handle your savings. 

### Features
- **Intuitive Dashboard:** A sleek and user-friendly dashboard gives you an instant overview of your financial status. Graphs and charts are created by using the [Google Charts API](https://developers.google.com/chart).
- **Transactions and Categories Management:** You can log new transactions and create new categories to organize your income/expenses.
- **User-Friendly Interface:** Intuitively designed with Bootstrap for a smooth user experience.
- **Secure Registration:** Login with your email and password for a personalized financial journey. Secure registration is provided through BCrypt password-hashing function.
- **Backed by SQL Server:** Robust database management with SQL Server for a reliable and scalable solution.

## Live Demonstration
Here are screenshots that show the Expense Tracker demo application in use.

**Dashboard**
//

**Login/Sign Up Page**
//

**Transactions Page**
//

**Categories Page**
//

**Savings Page**
//

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download) installed
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (optional but recommended)
  
### Dependencies
- [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net.git)
- [Entity Framework Core](https://github.com/dotnet/efcore.git)
- [Entity Framework Core Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools)
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer#readme-body-tab)

### Installation
1. Clone the repository:

   ```bash
   git clone https://github.com/Camila-Genco/ExpenseTracker.git your-project-name

2. Navigate to the project directory:

   ```bash
   cd your-project-name

3. Install dependencies:

   ```bash
   dotnet restore

4. Build the project:

   ```bash
   dotnet build

5. Run the application:

   ```bash
   dotnet run

The application will be available at https://localhost:5054

### Project Structure
- Controllers: Contains the controllers that handle requests.
- Views: Contains the views for rendering HTML responses.
- Models: Contains the data models used by the application.
- wwwroot: Contains static files like stylesheets, scripts, and images.
- Program.cs: Configures the application services and middleware.
  
### Configuration
- Update the appsettings.json file for environment-specific settings.
- Consider using environment variables for sensitive information.
  

