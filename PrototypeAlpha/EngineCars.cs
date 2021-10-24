using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeAlpha
{
    class EngineCars : Cars
    {
        private double tankSize;
        private double roadTax;
        private char fuelType;

        public EngineCars(string name, double range, double tankSize, double roadTax, char fuelType) : base(name, range)
        {
            this.tankSize = tankSize;
            this.roadTax = roadTax;
            this.fuelType = fuelType;
        }
        
        public double GetTankSize()
        {
            return tankSize;
        }
        public double GetRoadTax()
        {
            return roadTax;
        }
        public char GetFuelType()
        {
            return fuelType;
        }
        public void SetTankSize(double tankSize)
        {
            this.tankSize = tankSize;
        }
        public void SetRoadTax(double roadTax)
        {
            this.roadTax = roadTax;
        }
        public void SetFuelType(char fuelType)
        {
            this.fuelType = fuelType;
        }
    }
}
