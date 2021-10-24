using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeAlpha
{
    class ElectricCars : Cars
    {
        private double batterySize;
        public ElectricCars(string name, double range, double batterySize) : base(name, range)
        {
            this.batterySize = batterySize;
        }
        public double GetBatterySize()
        {
            return batterySize;
        }
        public void SetBatterySize(double batterySize)
        {
            this.batterySize = batterySize;
        }
    }
}
