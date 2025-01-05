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
            Console.WriteLine("Irrigation System gestart...");
            scheduler.Work();
            Console.WriteLine("Irrigation System voltooid.");
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("Een functie is nog niet geïmplementeerd.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Er is een fout opgetreden: {ex.Message}");
        }
    }
}
