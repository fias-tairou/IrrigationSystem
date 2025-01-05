using IrrigationSystem.Core;
using IrrigationSystem.Interfaces;
using IrrigationSystem.Models;
using IrrigationSystem.Services;
using Xunit;

namespace IrrigationSystem.IntegrationTests
{
    public class IrrigationSchedulerIntegrationTests
    {
        private readonly WeatherApi weatherApi;
        private readonly IrrigationControllerStub irrigationController;

        public IrrigationSchedulerIntegrationTests()
        {
            weatherApi = new WeatherApi("http://localhost:3000/weather");
            irrigationController = new IrrigationControllerStub();
        }

        [Fact]
        public void WhenRainfallAndSoilMoistureWithinThresholds_DoNothing()
        {
            var soilMoistureSensor = new SoilMoistureSensorStub(35.0); 
            var scheduler = new IrrigationScheduler(
                soilMoistureSensor,
                weatherApi,
                irrigationController,
                moistureThreshold: 30.0,
                rainfallThreshold: 10.0
            );

            scheduler.Work();

            Assert.False(irrigationController.IsEnabled); 
        }


        [Fact]
        public void WhenSoilMoistureBelowThreshold_StopIrrigation()
        {
            var soilMoistureSensor = new SoilMoistureSensorStub(20.0); 
            var scheduler = new IrrigationScheduler(
                soilMoistureSensor,
                weatherApi,
                irrigationController,
                moistureThreshold: 30.0,
                rainfallThreshold: 10.0
            );

            scheduler.Work();

            Assert.False(irrigationController.IsEnabled); 
        }

        [Fact]
        public void WhenRainfallAboveThreshold_StopIrrigation()
        {
            var soilMoistureSensor = new SoilMoistureSensorStub(35.0); 
            var scheduler = new IrrigationScheduler(
                soilMoistureSensor,
                weatherApi,
                irrigationController,
                moistureThreshold: 30.0,
                rainfallThreshold: 5.0
            );

            scheduler.Work();

            Assert.False(irrigationController.IsEnabled); 
        }


    }
}
