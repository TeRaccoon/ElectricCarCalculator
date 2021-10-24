using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeAlpha
{
    class Cars
    {
        protected string name;
        protected double range;
        public Cars(string name, double range)
        {
            this.name = name;
            this.range = range;
        }

        public string GetName()
        {
            return name;
        }
        public double GetRange()
        {
            return range;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public void SetRange(double range)
        {
            this.range = range;
        }
    }
}
