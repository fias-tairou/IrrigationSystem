using IrrigationSystem.Interfaces;

namespace IrrigationSystem.Services
{
    public class IrrigationControllerStub : IIrrigationController
    {
        public bool IsEnabled { get; private set; }

        public void Start()
        {
            IsEnabled = true; 
        }

        public void Stop()
        {
            IsEnabled = false;
        }
    }
}
