using BlazorWebApp.Data;

namespace BlazorWebApp.Service
{
    public class WeatherApiClient
    {
        private readonly HttpClient _httpClient;

        public WeatherApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast[]> GetWeatherAsync()
        {
            return await _httpClient.GetFromJsonAsync<WeatherForecast[]>("/weatherforecast");
        }
    }
}
