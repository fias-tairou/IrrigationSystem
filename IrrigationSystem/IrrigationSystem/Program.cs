namespace IrrigationSystem;

using IrrigationSystem.Core;
using IrrigationSystem.Services;

public class Program
{
    public static void Main()
    {
        // Maak de benodigde services aan
        var soilMoistureSensor = new SoilMoistureSensor(); // Mock in integratietests
        var weatherApi = new WeatherApi(); // Verwijst naar JSON op GitHub
        var irrigationController = new IrrigationController(); // Mock in integratietests

        // Maak de scheduler aan met drempelwaarden
        var scheduler = new IrrigationScheduler(
            soilMoistureSensor,
            weatherApi,
            irrigationController,
            moistureThreshold: 30.0, // Bodemvochtigheidsdrempel
            rainfallThreshold: 10.0  // Regenvaldrempel
        );

        try
        {
            // Voer de scheduler uit
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
