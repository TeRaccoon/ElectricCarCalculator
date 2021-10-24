using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace PrototypeAlpha
{
    class FuelPrices
    {
        private double petrolPrice;
        private double dieselPrice;
        private double ePricePerKWH;

        public FuelPrices(double petrolPrice, double dieselPrice, double ePricePerKWH)
        {
            this.petrolPrice = petrolPrice;
            this.dieselPrice = dieselPrice;
            this.ePricePerKWH = ePricePerKWH;
        }
        public double GetPetrolPrice()
        {
            return Convert.ToDouble(petrolPrice);
        }
        public double GetDieselPrice()
        {
            return dieselPrice;
        }
        public double GetElectricPrice()
        {
            return ePricePerKWH;
        }
        public void SetPetrolPrice(double petrolPrice)
        {
            this.petrolPrice = petrolPrice;
        }
        public void SetDieselPrice(double dieselPrice)
        {
            this.dieselPrice = dieselPrice;
        }
        public void SetElectricPrice(double ePricePerKWH)
        {
            this.ePricePerKWH = ePricePerKWH;
        }
    }
}
