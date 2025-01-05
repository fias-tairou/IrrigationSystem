using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationSystem.Interfaces
{
    public interface IIrrigationController
    {
        void Start();
        void Stop();
        bool IsEnabled { get; }
    }
}
