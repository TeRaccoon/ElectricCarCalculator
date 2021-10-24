using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeAlpha
{
    class HybridCars : Cars
    {
        private double tankSize;
        private double batterySize;
        private char fuelType;
        private double electricRange;
        private double engineRange;
        public HybridCars(string name, double range, double tankSize, double batterySize, char fuelType, double electricRange, double engineRange) : base(name, range)
        {
            this.tankSize = tankSize;
            this.batterySize = batterySize;
            this.fuelType = fuelType;
            this.electricRange = electricRange;
            this.engineRange = engineRange;
        }
        public double GetTankSize()
        {
            return tankSize;
        }
        public double GetBatterySize()
        {
            return batterySize;
        }
        public char GetFuelType()
        {
            return fuelType;
        }
        public double GetElectricRange()
        {
            return electricRange;
        }
        public double GetEngineRange()
        {
            return engineRange;
        }
        public void SetTankSize(double tankSize)
        {
            this.tankSize = tankSize;
        }
        public void SetBatterySize(double batterySize)
        {
            this.batterySize = batterySize;
        }
        public void SetFuelType(char fuelType)
        {
            this.fuelType = fuelType;
        }
        public void SetElectricRange(double electricRange)
        {
            this.electricRange = electricRange;
        }
        public void SetEngineRange(double engineRange)
        {
            this.engineRange = engineRange;
        }
    }
}
