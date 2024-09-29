# Weather API Service

A simple .NET Core API service that combines weather data from two external services (`Service A` and `Service B`). It fetches weather data from these services, combines the results, and returns a unified response. If either of the services is unavailable, it will return fallback test data.

## Features
- Fetches weather data from two external APIs.
- Converts temperature from Fahrenheit to Celsius.
- Combines weather data from both services.
- Returns fallback test data when external services are unavailable.
- Demonstrates usage of `HttpClient` for external API requests.

## Endpoints

### GET `/weather-combined`
Fetches combined weather data from two external services.

#### Request
```http
GET /weather-combined HTTP/1.1
Host: localhost:5000

Project Structure:

├── Controllers
│   └── WeatherController.cs       # Main controller handling API requests
├── Models
│   ├── WeatherAResponse.cs        # Model for Service A weather response
│   ├── WeatherBResponse.cs        # Model for Service B weather response
│   ├── Coord.cs                   # Model for latitude and longitude coordinates
│   └── WeatherDetails.cs          # Model for weather details (temperature, description)
├── Program.cs                     # Application entry point
├── Startup.cs                     # Configures services and HTTP client
└── README.md                      # Project documentation

Clone the repository:

git clone https://github.com/your-repo/weather-api-service.git
cd weather-api-service
