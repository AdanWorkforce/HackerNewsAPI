# Hacker News Best Stories API

A RESTful API implemented in ASP.NET Core to retrieve the best stories from the Hacker News API.

## Features

- Retrieves the best n stories from Hacker News sorted by descending score
- Implemented caching to improve performance and reduce external API calls
- Concurrency control to avoid overloading the Hacker News API
- Robust error handling
- Swagger documentation included

## Requirements

- .NET Core 3.1 SDK or higher
- Visual Studio 2019/2022 or any code editor

## How to Run

### Using Visual Studio
1. Clone the repository
2. Open `HackerNewsAPI.sln` in Visual Studio
3. Build the solution (`Ctrl + Shift + B`)
4. Run the application (`F5`)

### Using Command Line
```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/HackerNewsAPI.git
cd HackerNewsAPI

# Restore dependencies
dotnet restore

# Run the application
dotnet run