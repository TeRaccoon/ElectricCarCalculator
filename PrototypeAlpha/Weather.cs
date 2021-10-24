using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeAlpha
{
    class Weather
    {
        private string name;
        private bool on;
        private double rangeDeduction;
        
        public Weather(string name, bool on, double rangeDeduction)
        {
            this.name = name;
            this.on = on;
            this.rangeDeduction = rangeDeduction;
        }
        public string GetName()
        {
            return name;
        }
        public bool GetOn()
        {
            return on;
        }
        public double GetRangeDeduction()
        {
            return rangeDeduction;
        }

        public void SetOn(bool on)
        {
            this.on = on;
        }
    }
}
