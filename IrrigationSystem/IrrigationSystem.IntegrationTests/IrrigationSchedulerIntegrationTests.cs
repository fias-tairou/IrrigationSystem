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
            // Stel de Mockoon-URL in
            weatherApi = new WeatherApi("http://localhost:3000/weather");
            irrigationController = new IrrigationControllerStub();
        }

        [Fact]
        public void WhenRainfallAndSoilMoistureWithinThresholds_DoNothing()
        {
            // Arrange
            var soilMoistureSensor = new SoilMoistureSensorStub(35.0); // Stub voor bodemvochtigheid
            var scheduler = new IrrigationScheduler(
                soilMoistureSensor,
                weatherApi,
                irrigationController,
                moistureThreshold: 30.0,
                rainfallThreshold: 10.0
            );

            // Act
            scheduler.Work();

            // Assert
            Assert.False(irrigationController.IsEnabled); // Controleer dat irrigatie niet gestart is
        }


        [Fact]
        public void WhenSoilMoistureBelowThreshold_StartIrrigation()
        {
            // Arrange
            var soilMoistureSensor = new SoilMoistureSensorStub(20.0); // Stub voor bodemvochtigheid
            var scheduler = new IrrigationScheduler(
                soilMoistureSensor,
                weatherApi,
                irrigationController,
                moistureThreshold: 30.0,
                rainfallThreshold: 10.0
            );

            // Act
            scheduler.Work();

            // Assert
            Assert.True(irrigationController.IsEnabled); // Controleer dat irrigatie gestart is
        }

        [Fact]
        public void WhenRainfallAboveThreshold_StopIrrigation()
        {
            // Arrange
            var soilMoistureSensor = new SoilMoistureSensorStub(35.0); // Stub voor bodemvochtigheid
            var scheduler = new IrrigationScheduler(
                soilMoistureSensor,
                weatherApi,
                irrigationController,
                moistureThreshold: 30.0,
                rainfallThreshold: 5.0
            );

            // Act
            scheduler.Work();

            // Assert
            Assert.False(irrigationController.IsEnabled); // Controleer dat irrigatie gestopt is
        }


    }
}
