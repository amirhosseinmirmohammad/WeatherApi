using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WeatherApi.Models;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("weather-combined")]
        public async Task<IActionResult> GetCombinedWeather()
        {
            var weatherA = await GetWeatherFromServiceA();
            var weatherB = await GetWeatherFromServiceB();

            // Convert Units
            double celsiusFromFahrenheit = (weatherB.Fahrenheit - 32) * 5 / 9;

            var response = new
            {
                serviceA = new
                {
                    weatherA.Coord,
                    Weather = new
                    {
                        weatherA.Weather.Main,
                        weatherA.Weather.Description,
                        weatherA.Weather.Celsius 
                    }
                },
                serviceB = new
                {
                    weatherB.Main,
                    weatherB.Description,
                    Celsius = celsiusFromFahrenheit 
                }
            };

            return Ok(response);
        }

        private async Task<WeatherAResponse> GetWeatherFromServiceA()
        {
            try
            {
                var token = await LoginToServiceA();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.weatherapiA.com/v1/current.json?q=London");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var weatherData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<WeatherAResponse>(weatherData);
            }
            catch (HttpRequestException)
            {
                return new WeatherAResponse
                {
                    Coord = new Coord { Lon = 35.7074609, Lat = 51.1825053 },
                    Weather = new WeatherInfo
                    {
                        Main = "Clear",
                        Description = "Clear sky",
                        Celsius = 20
                    }
                };
            }
        }

        private async Task<WeatherBResponse> GetWeatherFromServiceB()
        {
            try
            {
                var token = await LoginToServiceB();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.weatherapiB.com/v1/current.json?q=London");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var weatherData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<WeatherBResponse>(weatherData);
            }
            catch (HttpRequestException)
            {
                return new WeatherBResponse
                {
                    Main = "Clear",
                    Description = "Clear sky",
                    Fahrenheit = 17 
                };
            }
        }

        private async Task<string> LoginToServiceA()
        {
            try
            {
                var loginPayload = new TokenRequest { User = "x", Pass = "Y" };
                var response = await _httpClient.PostAsJsonAsync("https://api.weatherapiA.com/v1/login", loginPayload);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var jsonResult = JsonSerializer.Deserialize<JsonElement>(result);
                return jsonResult.GetProperty("token").GetString();
            }
            catch (HttpRequestException)
            {
                return "test-token-A";
            }
        }

        private async Task<string> LoginToServiceB()
        {
            try
            {
                var loginPayload = new TokenRequest { User = "x", Pass = "Y" };
                var response = await _httpClient.PostAsJsonAsync("https://api.weatherapiB.com/v1/login", loginPayload);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                var jsonResult = JsonSerializer.Deserialize<JsonElement>(result);
                return jsonResult.GetProperty("token").GetString();
            }
            catch (HttpRequestException)
            {
                return "test-token-B"; 
            }
        }
    }
}
