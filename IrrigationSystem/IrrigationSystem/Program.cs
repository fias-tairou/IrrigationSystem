namespace IrrigationSystem;

using IrrigationSystem.Core;
using IrrigationSystem.Services;

public class Program
{
    public static void Main()
    {
        var soilMoistureSensor = new SoilMoistureSensor();
        var weatherApi = new WeatherApi();
        var irrigationController = new IrrigationController();

        var scheduler = new IrrigationScheduler(
            soilMoistureSensor,
            weatherApi,
            irrigationController,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        try
        {
            scheduler.Work();
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("Work() is not implemented yet.");
        }
    }
}
