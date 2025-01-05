using IrrigationSystem.Interfaces;

namespace IrrigationSystem.Services
{
    public class SoilMoistureSensorStub : ISoilMoistureSensor
    {
        private readonly double soilMoisture;

        public SoilMoistureSensorStub(double soilMoisture)
        {
            this.soilMoisture = soilMoisture;
        }

        public double GetSoilMoisture()
        {
            return soilMoisture;
        }
    }
}
