using IrrigationSystem.Interfaces;
using IrrigationSystem.Models;

namespace IrrigationSystem.Services
{
    public class WeatherApiStub : IWeatherApi
    {
        private readonly double rainfall;

        public WeatherApiStub(double rainfall)
        {
            this.rainfall = rainfall;
        }

        public WeatherData GetWeatherData()
        {
            return new WeatherData { Rainfall = rainfall };
        }
    }
}
