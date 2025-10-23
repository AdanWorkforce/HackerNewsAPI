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
git clone https://github.com/AdanWorkforce/HackerNewsAPI.git
cd HackerNewsAPI

# Restore dependencies
dotnet restore

# Run the application
dotnet run


API Usage
Endpoint
http
GET /api/stories/best?n=5
Parameters
n (optional): Number of stories to return (default: 10, maximum: 200)

Example Response
json
[
    {
        "title": "A uBlock Origin update was rejected from the Chrome Web Store",
        "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
        "postedBy": "ismaildonmez",
        "time": "2019-10-12T13:43:01+00:00",
        "score": 1716,
        "commentCount": 572
    }
]
Assumptions Made
Story Limit: Maximum of 200 stories to prevent API abuse

Cache Expiration: 5 minutes to balance data freshness and performance

Concurrency Control: Maximum 10 concurrent requests to Hacker News API

Error Handling: Individual story failures are handled gracefully

Filtering: Only stories of type "story" with non-empty titles are returned

Architecture
Framework: ASP.NET Core 3.1

Caching: IMemoryCache for efficient data storage

HTTP Client: HttpClient for external API calls

Concurrency: SemaphoreSlim to limit concurrent requests

Dependency Injection: Built-in IoC container

Enhancements (Given More Time)
Health Checks: Endpoints for service monitoring

Rate Limiting: Request limiting per client

Metrics & Monitoring: Application Insights integration

Background Services: Periodic cache updates

Resilience Policies: Retry mechanisms with Polly

Unit Tests: Comprehensive test coverage

Docker Support: Containerization

Configuration Management: Environment-specific settings

Logging: Structured logging with correlation IDs

API Versioning: Support for multiple API versions

Project Structure
text
HackerNewsAPI/
├── Controllers/          # API Controllers
├── Models/              # Data models
├── Services/            # Business logic services
├── Program.cs           # Application entry point
├── Startup.cs           # Startup configuration
└── README.md           # Project documentation
Technologies Used
ASP.NET Core 3.1

HttpClient

MemoryCache

Swagger/OpenAPI

SemaphoreSlim

System.Text.Json