namespace IrrigationSystem.Core;

using IrrigationSystem.Interfaces;

public class IrrigationScheduler
{
    private readonly ISoilMoistureSensor soilMoistureSensor;
    private readonly IWeatherApi weatherApi;
    private readonly IIrrigationController irrigationController;
    private readonly double MoistureThreshold;
    private readonly double RainfallThreshold;

    private int failures = 0; 
    private const int MaxFailures = 3; 

    public bool IsInSafeMode => failures >= MaxFailures; 

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
        if (IsInSafeMode)
        {
            Console.WriteLine("System is in Safe Mode. Irrigation stopped.");
            irrigationController.Stop();
            return;
        }

        try
        {
            var weather = weatherApi.GetWeatherData();
            var soilMoisture = soilMoistureSensor.GetSoilMoisture();

            failures = 0;

            if (weather.Rainfall == 0 && soilMoisture >= MoistureThreshold)
            {
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

            if (weather.Rainfall >= RainfallThreshold)
            {
                irrigationController.Stop();
                return;
            }

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
