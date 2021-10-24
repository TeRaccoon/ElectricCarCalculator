using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeAlpha
{
    class Settings
    {
        private int weatherIndex;
        private string vehicleType;
        private int vehicleIndex;
        public Settings(int weatherIndex, string vehicleType, int vehicleIndex)
        {
            this.weatherIndex = weatherIndex;
            this.vehicleType = vehicleType;
            this.vehicleIndex = vehicleIndex;
        }
        public int GetWeatherIndex()
        {
            return weatherIndex;
        }
        public string GetVehicleType()
        {
            return vehicleType;
        }
        public int GetVehicleIndex()
        {
            return vehicleIndex;
        }
        public void SetWeatherIndex(int weatherIndex)
        {
            this.weatherIndex = weatherIndex;
        }
        public void SetVehicleType(string vehicleType)
        {
            this.vehicleType = vehicleType;
        }
        public void SetVehicleIndex(int vehicleIndex)
        {
            this.vehicleIndex = vehicleIndex;
        }
    }
}
