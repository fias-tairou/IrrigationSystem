using IrrigationSystem.Interfaces;
using IrrigationSystem.Models;
using System.Net.Http;
using System.Text.Json;

namespace IrrigationSystem.Services
{
    public class WeatherApi : IWeatherApi
    {
        // Verwijzing naar de JSON-file op GitHub
        private string ApiUrl = "https://raw.githubusercontent.com/fias-tairou/rain_api/refs/heads/main/weather.json";
        private string v;

        public WeatherApi()
        {
        }

        public WeatherApi(string v)
        {
            ApiUrl = v;
        }

        public WeatherData GetWeatherData()
        {
            try
            {
                using HttpClient client = new HttpClient();
                var response = client.GetStringAsync(ApiUrl).Result;

                // Deserialiseer de JSON-respons
                var jsonData = JsonSerializer.Deserialize<WeatherApiResponse>(response);

                if (jsonData?.Data != null && jsonData.Data.Any())
                {
                    // Neem de eerste entry
                    return jsonData.Data.First();
                }

                throw new Exception("Geen geldige data beschikbaar in API-respons.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Fout bij ophalen van weerdata: {ex.Message}");
            }
        }
    }

    // Hulpmiddelen om JSON te deserialiseren
    public class WeatherApiResponse
    {
        public List<WeatherData> Data { get; set; }
    }
}
