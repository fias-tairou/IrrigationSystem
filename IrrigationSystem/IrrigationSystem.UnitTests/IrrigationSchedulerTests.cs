namespace IrrigationSystem.UnitTests;

using IrrigationSystem.Core;
using IrrigationSystem.Interfaces;
using IrrigationSystem.Models;
using Moq;

public class IrrigationSchedulerTests
{
    [Fact]
    public void Work_WhenNoRainfallAndMoistureAboveThreshold_ShouldDoNothing()
    {
        var weatherMock = new Mock<IWeatherApi>();
        weatherMock.Setup(api => api.GetWeatherData())
                   .Returns(new WeatherData { Rainfall = 0 });

        var soilSensorMock = new Mock<ISoilMoistureSensor>();
        soilSensorMock.Setup(sensor => sensor.GetSoilMoisture())
                      .Returns(40); 

        var irrigationMock = new Mock<IIrrigationController>();

        var scheduler = new IrrigationScheduler(
            soilSensorMock.Object,
            weatherMock.Object,
            irrigationMock.Object,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        scheduler.Work();

        irrigationMock.Verify(controller => controller.Start(), Times.Never);
        irrigationMock.Verify(controller => controller.Stop(), Times.Never);
    }

    [Fact]
    public void Work_WhenNoRainfallAndMoistureEqualsThreshold_ShouldDoNothing()
    {
        var weatherMock = new Mock<IWeatherApi>();
        weatherMock.Setup(api => api.GetWeatherData())
                   .Returns(new WeatherData { Rainfall = 0 });

        var soilSensorMock = new Mock<ISoilMoistureSensor>();
        soilSensorMock.Setup(sensor => sensor.GetSoilMoisture())
                      .Returns(30); 

        var irrigationMock = new Mock<IIrrigationController>();

        var scheduler = new IrrigationScheduler(
            soilSensorMock.Object,
            weatherMock.Object,
            irrigationMock.Object,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        scheduler.Work();

        irrigationMock.Verify(controller => controller.Start(), Times.Never);
        irrigationMock.Verify(controller => controller.Stop(), Times.Never);
    }

    [Fact]
    public void Work_WhenRainfallEqualsThreshold_ShouldStopIrrigation()
    {
        var weatherMock = new Mock<IWeatherApi>();
        weatherMock.Setup(api => api.GetWeatherData())
                   .Returns(new WeatherData { Rainfall = 10.0 }); 

        var soilSensorMock = new Mock<ISoilMoistureSensor>();
        soilSensorMock.Setup(sensor => sensor.GetSoilMoisture())
                      .Returns(40.0); 

        var irrigationMock = new Mock<IIrrigationController>();

        var scheduler = new IrrigationScheduler(
            soilSensorMock.Object,
            weatherMock.Object,
            irrigationMock.Object,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        scheduler.Work();

        irrigationMock.Verify(controller => controller.Start(), Times.Never);
        irrigationMock.Verify(controller => controller.Stop(), Times.Once);
    }

    [Fact]
    public void Work_WhenSoilMoistureBelowThreshold_ShouldStartIrrigation()
    {
        var weatherMock = new Mock<IWeatherApi>();
        weatherMock.Setup(api => api.GetWeatherData())
                   .Returns(new WeatherData { Rainfall = 0 }); 

        var soilSensorMock = new Mock<ISoilMoistureSensor>();
        soilSensorMock.Setup(sensor => sensor.GetSoilMoisture())
                      .Returns(20.0); 

        var irrigationMock = new Mock<IIrrigationController>();

        var scheduler = new IrrigationScheduler(
            soilSensorMock.Object,
            weatherMock.Object,
            irrigationMock.Object,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        scheduler.Work();

        irrigationMock.Verify(controller => controller.Start(), Times.Once);
        irrigationMock.Verify(controller => controller.Stop(), Times.Never);
    }

    [Fact]
    public void Work_WhenRainfallExceedsThreshold_ShouldStopIrrigation()
    {
        var weatherMock = new Mock<IWeatherApi>();
        weatherMock.Setup(api => api.GetWeatherData())
                   .Returns(new WeatherData { Rainfall = 15.0 }); 

        var soilSensorMock = new Mock<ISoilMoistureSensor>();
        soilSensorMock.Setup(sensor => sensor.GetSoilMoisture())
                      .Returns(40.0); 

        var irrigationMock = new Mock<IIrrigationController>();

        var scheduler = new IrrigationScheduler(
            soilSensorMock.Object,
            weatherMock.Object,
            irrigationMock.Object,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        scheduler.Work();

        irrigationMock.Verify(controller => controller.Start(), Times.Never);
        irrigationMock.Verify(controller => controller.Stop(), Times.Once);
    }

    [Fact]
    public void Work_WhenRainfallAndMoistureEqualThreshold_ShouldStopIrrigation()
    {
        var weatherMock = new Mock<IWeatherApi>();
        weatherMock.Setup(api => api.GetWeatherData())
                   .Returns(new WeatherData { Rainfall = 10.0 }); 

        var soilSensorMock = new Mock<ISoilMoistureSensor>();
        soilSensorMock.Setup(sensor => sensor.GetSoilMoisture())
                      .Returns(30.0); 

        var irrigationMock = new Mock<IIrrigationController>();

        var scheduler = new IrrigationScheduler(
            soilSensorMock.Object,
            weatherMock.Object,
            irrigationMock.Object,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        scheduler.Work();

        irrigationMock.Verify(controller => controller.Start(), Times.Never);
        irrigationMock.Verify(controller => controller.Stop(), Times.Once);
    }

    [Fact]
    public void Work_WhenApiFailsMultipleTimes_ShouldEnterSafeMode()
    {
        var weatherMock = new Mock<IWeatherApi>();
        weatherMock.Setup(api => api.GetWeatherData())
                   .Throws(new Exception()); 

        var soilSensorMock = new Mock<ISoilMoistureSensor>();
        soilSensorMock.Setup(sensor => sensor.GetSoilMoisture())
                      .Returns(35.0); 

        var irrigationMock = new Mock<IIrrigationController>();

        var scheduler = new IrrigationScheduler(
            soilSensorMock.Object,
            weatherMock.Object,
            irrigationMock.Object,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        for (int i = 0; i < 3; i++)
        {
            scheduler.Work();
        }

        Assert.True(scheduler.IsInSafeMode, "System should be in Safe Mode.");
        irrigationMock.Verify(controller => controller.Stop(), Times.Once);
    }

    [Fact]
    public void Work_WhenApiRecovers_ShouldExitSafeMode()
    {
        var weatherMock = new Mock<IWeatherApi>();
        weatherMock.SetupSequence(api => api.GetWeatherData())
                   .Throws(new Exception()) 
                   .Throws(new Exception())
                   .Returns(new WeatherData { Rainfall = 0 }); 

        var soilSensorMock = new Mock<ISoilMoistureSensor>();
        soilSensorMock.Setup(sensor => sensor.GetSoilMoisture())
                      .Returns(20.0); 

        var irrigationMock = new Mock<IIrrigationController>();

        var scheduler = new IrrigationScheduler(
            soilSensorMock.Object,
            weatherMock.Object,
            irrigationMock.Object,
            moistureThreshold: 30.0,
            rainfallThreshold: 10.0
        );

        for (int i = 0; i < 3; i++)
        {
            scheduler.Work();
        }

        scheduler.Work();

        Assert.False(scheduler.IsInSafeMode, "System should exit Safe Mode after API recovers.");
        irrigationMock.Verify(controller => controller.Start(), Times.Once);
    }
}
