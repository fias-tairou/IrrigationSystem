using Xunit.Gherkin.Quick;
using IrrigationSystem.Core;
using IrrigationSystem.Services;

namespace IrrigationSystem.AcceptanceTests.StepDefinitions
{
    [FeatureFile("./Features/IrrigationSystem.feature")]
    public class IrrigationSystemStepDefinitions : Feature
    {
        private double soilMoisture;
        private double rainfall;
        private IrrigationControllerStub irrigationController;
        private IrrigationScheduler scheduler;

        [Given(@"the soil moisture is below the threshold")]
        public void GivenSoilMoistureIsBelowThreshold()
        {
            soilMoisture = 20.0; 
        }

        [Given(@"the rainfall is below the threshold")]
        public void GivenRainfallIsBelowThreshold()
        {
            rainfall = 5.0; 
        }

        [When(@"the irrigation scheduler runs")]
        public void WhenTheIrrigationSchedulerRuns()
        {
            var soilMoistureSensor = new SoilMoistureSensorStub(soilMoisture);
            var weatherApi = new WeatherApiStub(rainfall);
            irrigationController = new IrrigationControllerStub();

            scheduler = new IrrigationScheduler(
                soilMoistureSensor,
                weatherApi,
                irrigationController,
                moistureThreshold: 30.0,
                rainfallThreshold: 10.0
            );

            scheduler.Work();
        }

        [Then(@"the irrigation system should be enabled")]
        public void ThenTheIrrigationSystemShouldBeEnabled()
        {
            Assert.True(irrigationController.IsEnabled); 
        }
    }
}
