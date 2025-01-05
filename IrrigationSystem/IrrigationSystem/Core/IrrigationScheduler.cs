namespace IrrigationSystem.Core;

using IrrigationSystem.Interfaces;

public class IrrigationScheduler
{
    private readonly ISoilMoistureSensor soilMoistureSensor;
    private readonly IWeatherApi weatherApi;
    private readonly IIrrigationController irrigationController;
    private readonly double MoistureThreshold;
    private readonly double RainfallThreshold;

    private int failures = 0; // Foutenteller
    private const int MaxFailures = 3; // Maximaal aantal fouten voor Safe Mode

    public bool IsInSafeMode => failures >= MaxFailures; // Eigenschap voor Safe Mode

    public IrrigationScheduler(
        ISoilMoistureSensor soilMoistureSensor,
        IWeatherApi weatherApi,
        IIrrigationController irrigationController,
        double moistureThreshold,
        double rainfallThreshold)
    {
        this.soilMoistureSensor = soilMoistureSensor;
        this.weatherApi = weatherApi;
        this.irrigationController = irrigationController;
        this.MoistureThreshold = moistureThreshold;
        this.RainfallThreshold = rainfallThreshold;
    }

    public void Work()
    {
        // Controleer eerst of we in Safe Mode zijn
        if (IsInSafeMode)
        {
            Console.WriteLine("System is in Safe Mode. Irrigation stopped.");
            irrigationController.Stop();
            return;
        }

        try
        {
            // Haal gegevens op
            var weather = weatherApi.GetWeatherData();
            var soilMoisture = soilMoistureSensor.GetSoilMoisture();

            // Reset foutenteller bij succes
            failures = 0;

            // Beslissingslogica - Huidige logica wordt behouden
            if (weather.Rainfall == 0 && soilMoisture >= MoistureThreshold)
            {
                // Doe niets
                return;
            }

            if (weather.Rainfall == RainfallThreshold)
            {
                irrigationController.Stop();
                return;
            }

            if (weather.Rainfall > RainfallThreshold)
            {
                irrigationController.Stop();
                return;
            }

            if (soilMoisture < MoistureThreshold)
            {
                irrigationController.Start();
                return;
            }

            // Prioriteit 1: Regenval
            if (weather.Rainfall >= RainfallThreshold)
            {
                irrigationController.Stop();
                return;
            }

            // Prioriteit 2: Bodemvochtigheid
            if (soilMoisture < MoistureThreshold)
            {
                irrigationController.Start();
                return;
            }

            irrigationController.Stop();
        }
        catch
        {
            failures++;
            Console.WriteLine($"Error occurred. Failure count: {failures}");

            if (failures >= MaxFailures)
            {
                Console.WriteLine("Safe Mode activated: Too many failures.");
                irrigationController.Stop();
            }
        }
    }
}
