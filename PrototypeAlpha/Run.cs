using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace PrototypeAlpha
{
    class Run
    {
        private EngineCars[] EnCar;
        private HybridCars[] HyCar;
        private ElectricCars[] ElCar;
        private Settings SettingsData;
        private FuelPrices FP;
        private Weather[] Weathers = new Weather[5];
        string[] vehicleData = new string[@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data.txt".Length + 10];
        public Run()
        {
            LoadVehicleData();
            FP = new FuelPrices(0, 0, 12.827);
            SettingsData = new Settings(0, "ELECTRIC", 0);
            Weathers[0] = new Weather("Default", true, 1);
            Weathers[1] = new Weather("Rain", false, 0.95);
            Weathers[2] = new Weather("Snow", false, 0.59);
            Weathers[3] = new Weather("Ice", false, 0.62);
            Weathers[4] = new Weather("Sun", false, 0.85);

            LoadSettings();
            SetPrices();
            Start();
        }

        public void LoadVehicleData()
        {
            int k = 3;
            string[] uploadData = File.ReadAllLines(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data.txt");
            for (int i = 0; i < uploadData.Length; i++)
            {
                vehicleData[i] = uploadData[i];
            }
            int engineCarAmount = Convert.ToInt32(vehicleData[0]); //so it knows how many times it has to loop while loading data of the text file, the start of the file contains the amount of each vehicle
            int electricCarAmount = Convert.ToInt32(vehicleData[1]);
            int hybridCarAmount = Convert.ToInt32(vehicleData[2]);
            EnCar = new EngineCars[engineCarAmount + 1]; //has to be instantiated here because I need the sizes to change as you can add more cars so the +1 is preparing oncase that happens
            ElCar = new ElectricCars[electricCarAmount + 1];
            HyCar = new HybridCars[hybridCarAmount + 1];
            for (int i = 0; i < engineCarAmount; i++)
            {
                EnCar[i] = new EngineCars("", 0, 0, 0, ' ');
            }
            for (int i = 0; i < electricCarAmount; i++)
            {
                ElCar[i] = new ElectricCars("", 0, 0);
            }
            for (int i = 0; i < hybridCarAmount; i++)
            {
                HyCar[i] = new HybridCars("", 0, 0, 0, ' ', 0, 0);
            }
            for (int index = 0; index < engineCarAmount; index++)
            {
                EnCar[index].SetName(vehicleData[k]);
                k++;
                EnCar[index].SetRange(Convert.ToInt32(vehicleData[k]));
                k++;
                EnCar[index].SetTankSize(Convert.ToDouble(vehicleData[k]));
                k++;
                EnCar[index].SetRoadTax(Convert.ToInt32(vehicleData[k]));
                k++;
                EnCar[index].SetFuelType(Convert.ToChar(vehicleData[k]));
                k++;
            }
            for (int index = 0; index < electricCarAmount; index++)
            {
                ElCar[index].SetName(vehicleData[k]);
                k++;
                ElCar[index].SetRange(Convert.ToInt32(vehicleData[k]));
                k++;
                ElCar[index].SetBatterySize(Convert.ToDouble(vehicleData[k]));
                k++;
            }
            for (int index = 0; index < hybridCarAmount; index++)
            {
                HyCar[index].SetName(vehicleData[k]);
                k++;
                HyCar[index].SetRange(Convert.ToInt32(vehicleData[k]));
                k++;
                HyCar[index].SetTankSize(Convert.ToDouble(vehicleData[k]));
                k++;
                HyCar[index].SetBatterySize(Convert.ToDouble(vehicleData[k]));
                k++;
                HyCar[index].SetFuelType(Convert.ToChar(vehicleData[k]));
                k++;
                HyCar[index].SetElectricRange(Convert.ToDouble(vehicleData[k]));
                k++;
                HyCar[index].SetEngineRange(Convert.ToDouble(vehicleData[k]));
                k++;
            }
        }
        public void LoadSettings() //loads all the default settings information off the text file so you don't have to keep resetting them
        {
            string[] data = File.ReadAllLines(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Settings.txt");
            SettingsData.SetWeatherIndex(Convert.ToInt32(data[0]));
            SettingsData.SetVehicleType(data[1]);
            SettingsData.SetVehicleIndex(Convert.ToInt32(data[2]));
        }
        public void SetPrices() //takes the average fuel prices off the website below
        {
            WebClient client = new WebClient();
            string petrolPrice;
            string dieselPrice;
            try
            {
                string downloadString = client.DownloadString("https://www.allstarcard.co.uk/tools/uk-fuel-prices/");
                string[] html = downloadString.Split('\n');
                string[] prices = new string[4];
                int j = 0;
                for (int i = 0; i < html.Length; i++) //finds the values because it's a certain amount of lines between the <h2> tags
                {
                    if (html[i].Contains("<h2>"))
                    {
                        if (html[i + 2].Contains("</h2>"))
                        {
                            prices[j] = html[i + 1];
                            j++;
                        }
                    }
                }
                petrolPrice = prices[0];
                dieselPrice = prices[1];
                Regex rgx = new Regex("[^0-9.]");
                petrolPrice = rgx.Replace(petrolPrice, "");
                dieselPrice = rgx.Replace(dieselPrice, ""); //strips the whole raw html text down so it's just the values and no extra spaces etc
            }
            catch (Exception)
            {
                DialogResult input = MessageBox.Show("Unable to reach allstarcard.co.uk/tools/uk-fuel-prices/ to retrieve fuel price information, would you like to enter your own fuel prices? (if not, rough values will be used instead)", "Error", MessageBoxButtons.YesNo);
                if (DialogResult.Yes == input)
                {
                    Console.Write("Enter petrol price in P per Litre: ");
                    petrolPrice = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a suitable value", 1, 1000));
                    Console.Write("Enter diesel price in P per Litre: ");
                    dieselPrice = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a suitable value", 1, 1000));
                    Console.Clear();
                }
                else
                {
                    petrolPrice = "125.5";
                    dieselPrice = "129.1";
                }
            }
            FP.SetPetrolPrice(Convert.ToDouble(petrolPrice));
            FP.SetDieselPrice(Convert.ToDouble(dieselPrice));
        }
        public void Start()
        {
            int input = 0;
            while (input != 5)
            {
                DisplayData();
                Console.WriteLine("Please select one of the following options (1-5):");
                Console.WriteLine("1 - Range check");
                Console.WriteLine("2 - Running costs of an electric vehicle compared to a engine per mile");
                Console.WriteLine("3 - Journey efficiency (only for engine and electric vehicles)");
                Console.WriteLine("4 - Settings");
                Console.WriteLine("5 - Exit");
                input = IntValidation(Console.ReadLine(), "Please enter a number between 1 and 5", 1, 5);
                switch (input)
                {
                    case 1:
                        Console.WriteLine("You can travel roughly " + RangeCheck() + " miles");
                        break;

                    case 2:
                        Console.WriteLine(PriceCompare());
                        break;

                    case 3:
                        Console.WriteLine(JourneyEfficiency());
                        break;

                    case 4:
                        Settings();
                        break;

                    case 5:
                        Console.WriteLine("Exiting Program");
                        break;
                }
                Console.WriteLine("\n" + "Press enter to continue");
                Console.ReadLine();
                Console.Clear();
            }
        }
        public void DisplayData() //displays the menu
        {
            int engineCarAmount = Convert.ToInt32(vehicleData[0]);
            int electricCarAmount = Convert.ToInt32(vehicleData[1]);
            int hybridCarAmount = Convert.ToInt32(vehicleData[2]);
            Console.WriteLine("Use this software to work out expenses of running an electric car vs an engine car");
            Console.Write("Current default vehicle is: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (SettingsData.GetVehicleType() == "ENGINE") //gets the default vehicle name
            {
                Console.WriteLine(EnCar[SettingsData.GetVehicleIndex()].GetName());
            }
            if (SettingsData.GetVehicleType() == "ELECTRIC")
            {
                Console.WriteLine(ElCar[SettingsData.GetVehicleIndex()].GetName());
            }
            if (SettingsData.GetVehicleType() == "HYBRID")
            {
                Console.WriteLine(HyCar[SettingsData.GetVehicleIndex()].GetName());
            }
            Console.ResetColor();
            Console.Write("\nWeather is set to: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;  //changes colour so it can be easier differentiated from the menu options 
            Console.WriteLine(Weathers[SettingsData.GetWeatherIndex()].GetName() + "\n"); //writes the default weather
            Console.ResetColor();
            Console.WriteLine("Current cars available:");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Engine:");
            for (int i = 0; i < engineCarAmount; i++)
            {
                Console.WriteLine(i + " - " + EnCar[i].GetName());
            }
            Console.Write("\n\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Electric:");
            for (int i = 0; i < electricCarAmount; i++)
            {
                Console.WriteLine(i + " - " + ElCar[i].GetName());
            }
            Console.Write("\n\n");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("Hybrid:");
            for (int i = 0; i < hybridCarAmount; i++)
            {
                Console.WriteLine(i + " - " + HyCar[i].GetName());
            }
            Console.Write("\n\n");
            Console.ResetColor();
        }
        public void Settings() //menu for all the settings options
        {
            Console.WriteLine("\n");
            int selectedWeatherIndex = -1;
            int selectedVehicleIndex = -1;
            string vehicleType = "";
            Console.WriteLine("Settings - Please select one of the following options:");
            Console.WriteLine("1 - Change default weather");
            Console.WriteLine("2 - Change default vehicle");
            Console.WriteLine("3 - Statistics");
            Console.WriteLine("4 - Sort vehicles by range (highest to lowest)");
            Console.WriteLine("5 - Create a vehicle to Simulate");
            Console.WriteLine("6 - Restore all to default");
            Console.WriteLine("7 - Update restore file");
            Console.WriteLine("8 - Exit");
            int input = IntValidation(Console.ReadLine(), "Select an option between 1 and 8", 1, 8);
            switch (input)
            {
                case 1: //allows the user to change the default weather
                    Console.WriteLine("These are the current supported weathers available, select either a number 1-5 to choose which weather you want active:");
                    Console.WriteLine("1 - Default");
                    Console.WriteLine("2 - Rain");
                    Console.WriteLine("3 - Snow");
                    Console.WriteLine("4 - Ice");
                    Console.WriteLine("5 - Sun");
                    Console.WriteLine("6 - Exit");
                    selectedWeatherIndex = IntValidation(Console.ReadLine(), "Please select which weather you want to select, 1 - 5", 1, 5) - 1;
                    Weathers[SettingsData.GetWeatherIndex()].SetOn(false);
                    Weathers[selectedWeatherIndex].SetOn(true);
                    Console.WriteLine("You have selected " + Weathers[selectedWeatherIndex].GetName());
                    break;

                case 2: //allows the user to set a default vehicle
                    Console.WriteLine("Enter the desired vehicle type (eg Electric) followed by the vehicle number from the list above:");
                    vehicleType = Console.ReadLine().ToUpper();
                    while (vehicleType != "ENGINE" && vehicleType != "ELECTRIC" && vehicleType != "HYBRID")
                    {
                        Console.WriteLine("Enter one of the listed vehicle types: \n Engine \n Electric \n Hybrid");
                        vehicleType = Console.ReadLine().ToUpper();
                    }
                    if (vehicleType == "ENGINE")
                    {
                        selectedVehicleIndex = IntValidation(Console.ReadLine(), "Please select an engine vehicle, 0 - " + EnCar.Length + " from the listed above", 0, EnCar.Length - 1);
                        Console.WriteLine("You have selected " + EnCar[selectedVehicleIndex].GetName(), " as the default vehicle");
                    }
                    if (vehicleType == "ELECTRIC")
                    {
                        selectedVehicleIndex = IntValidation(Console.ReadLine(), "Please select an electric vehicle, 0 - " + ElCar.Length + "from the listed above", 0, ElCar.Length - 1);
                        Console.WriteLine("You have selected " + ElCar[selectedVehicleIndex].GetName() + " as the default vehicle");
                    }
                    if (vehicleType == "HYBRID")
                    {
                        selectedVehicleIndex = IntValidation(Console.ReadLine(), "Please select a hybrid vehicle, 0 - " + HyCar.Length + " from the listed above", 0, HyCar.Length - 1);
                        Console.WriteLine("You have selected " + HyCar[selectedVehicleIndex].GetName() + " as the default vehicle");
                    }
                    break;

                case 3: //allows the user to view all vehicle and weather values such as range reducer from weather
                    Statistics();
                    break;

                case 4: //sorts all vehicles from lowest range to highest within their vehicle types
                    SortVehicleData(Convert.ToInt32(vehicleData[0]), Convert.ToInt32(vehicleData[1]), Convert.ToInt32(vehicleData[2]));
                    break;

                case 5: //allows the user to create their own custom vehicle
                    CreateVehicle();
                    break;

                case 6:
                    RestoreToDefault();
                    break;

                case 7:
                    UpdateRestore();
                    break;

                case 8:
                    Console.WriteLine("Exiting settings");
                    break;
            }
            SaveSettings(selectedWeatherIndex, vehicleType, selectedVehicleIndex); //saves anything that may have been changed
        }
        public void SaveSettings(int weatherIndex, string vehicleType, int defaultVehicle) //updates the settings text file
        {
            if (weatherIndex == -1)
            {
                weatherIndex = SettingsData.GetWeatherIndex();
            }
            if (defaultVehicle == -1)
            {
                defaultVehicle = SettingsData.GetVehicleIndex();
                vehicleType = SettingsData.GetVehicleType();
            }
            string[] saveData = { Convert.ToString(weatherIndex), vehicleType, Convert.ToString(defaultVehicle) }; //puts the settings data in an array to be written to a text file
            File.WriteAllLines(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Settings.txt", saveData);
            LoadSettings();
        }
        public void Statistics() //prints out all of the vehicle data while adding headers to make it understandable to the user
        {
            Console.WriteLine("Here are the following statistics about the weather and vehicles");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Range deductions for each weather (These represent the percent it will take off based on HVAC and effect on batteries)");
            for (int i = 0; i < Weathers.Length; i++)
            {
                Console.WriteLine(Weathers[i].GetName() + " - " + Weathers[i].GetRangeDeduction());
            }
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\nEngine vehicle data:");
            for (int i = 0; i < EnCar.Length - 1; i++)
            {
                Console.WriteLine("Name " + EnCar[i].GetName());
                Console.WriteLine("Range (miles) " + EnCar[i].GetRange());
                Console.WriteLine("Tank size (litres) " + EnCar[i].GetTankSize());
                Console.WriteLine("Road tax (GBP) " + EnCar[i].GetRoadTax());
                Console.WriteLine("Fuel type (P for petrol, D for diesel) " + EnCar[i].GetFuelType() + "\n");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nElectric vehicle data:");
            for (int i = 0; i < ElCar.Length - 1; i++)
            {
                Console.WriteLine("Name " + ElCar[i].GetName());
                Console.WriteLine("Range (miles) " + ElCar[i].GetRange());
                Console.WriteLine("Battery Size (kWH) " + ElCar[i].GetBatterySize() + "\n");
            }
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\nHybrid vehicle data:");
            for (int i = 0; i < HyCar.Length - 1; i++)
            {
                Console.WriteLine("Name " + HyCar[i].GetName());
                Console.WriteLine("Range (miles) " + HyCar[i].GetRange());
                Console.WriteLine("Tank size (L) " + HyCar[i].GetTankSize());
                Console.WriteLine("Battery size (kWH) " + HyCar[i].GetBatterySize());
                Console.WriteLine("Fuel type (P for petrol, D for diesel) " + HyCar[i].GetFuelType());
                Console.WriteLine("Electric range (Miles) " + HyCar[i].GetElectricRange());
                Console.WriteLine("Engine range (Miles) " + HyCar[i].GetEngineRange() + "\n");
            }
            Console.ResetColor();
        }
        public double RangeCheck() //calculates your rough expected range depending on battery percent and weather
        {
            double range = 0;
            if (SettingsData.GetVehicleType() == "ELECTRIC")
            {
                range = ElCar[SettingsData.GetVehicleIndex()].GetRange(); //gets vehicle range
                Console.WriteLine("Enter your current battery percent to the nearest whole number");
                double batteryPercent = IntValidation(Console.ReadLine(), "Please enter a battery percent, 1 - 100%", 1, 100);
                range = range / 100 * batteryPercent * Weathers[SettingsData.GetWeatherIndex()].GetRangeDeduction(); // calculates range by doing actual range / 100 * battery percent * range deduction from active weather
            }
            if (SettingsData.GetVehicleType() == "ENGINE")
            {
                range = EnCar[SettingsData.GetVehicleIndex()].GetRange();
                Console.WriteLine("Enter the rough amount of fuel in your vehicle in litres");
                double fuelAmount = IntValidation(Console.ReadLine(), "Enter a suitable fuel amount for your vehicle", 1, Convert.ToInt32(EnCar[SettingsData.GetVehicleIndex()].GetTankSize()));
                range = range / 100 * (fuelAmount / EnCar[SettingsData.GetVehicleIndex()].GetTankSize() * 100) * Weathers[SettingsData.GetWeatherIndex()].GetRangeDeduction(); //calculates range by doing vehicles actual range / tank size * 100 * range deduction from active weather
            }
            if (SettingsData.GetVehicleType() == "HYBRID")
            {
                double enRange = HyCar[SettingsData.GetVehicleIndex()].GetEngineRange();
                double elRange = HyCar[SettingsData.GetVehicleIndex()].GetElectricRange();
                Console.WriteLine("Enter the rough amount of fuel in your vehicle in litres");
                double fuelAmount = IntValidation(Console.ReadLine(), "Enter a suitable fuel amount for your vehicle", 1, Convert.ToInt32(HyCar[SettingsData.GetVehicleIndex()].GetTankSize()));
                Console.WriteLine("Enter your current battery percent to the nearest whole number");
                double batteryPercent = IntValidation(Console.ReadLine(), "Please enter a battery percent, 1 - 100%", 1, 100);
                range = Math.Round((enRange / 100 * (fuelAmount / HyCar[SettingsData.GetVehicleIndex()].GetTankSize() * 100) * Weathers[SettingsData.GetWeatherIndex()].GetRangeDeduction()) + (elRange / 100 * batteryPercent * Weathers[SettingsData.GetWeatherIndex()].GetRangeDeduction()));
            }
            return range;
        }
        public string PriceCompare() //compares price per mile for an electric and an engine car by using the averge fuel price per litre
        {
            int selectedElectricCar;
            int selectedEngineCar;
            double costPerMileFuel;

            Console.WriteLine("Select an electric vehicle by entering the index (number to the left) of the vehicle from the list above");
            selectedElectricCar = IntValidation(Console.ReadLine(), "Please select an electric vehicle, 0 - " + Convert.ToString(Convert.ToInt32(vehicleData[1]) - 1), 0, Convert.ToInt32(vehicleData[0]) - 1);
            Console.WriteLine("Select an engine vehicle by entering the index (number to the left) of the vehicle from the list above");
            selectedEngineCar = IntValidation(Console.ReadLine(), "Please select an engine vehicle, 0 - " + Convert.ToString(Convert.ToInt32(vehicleData[0]) - 1), 0, Convert.ToInt32(vehicleData[0]) - 1);
            double costPerMileElectric = ElCar[selectedElectricCar].GetBatterySize() / ElCar[selectedElectricCar].GetRange() * FP.GetElectricPrice(); //get price per mile from battery size and range 
            if (EnCar[selectedEngineCar].GetFuelType() == 'D') //calculates cost per mile for the engine vehicle depending on fuel type buy doing the vehicles tank size / range to get miles per litre then multiplying by fuel price
            {
                costPerMileFuel = EnCar[selectedEngineCar].GetTankSize() / EnCar[selectedEngineCar].GetRange() * FP.GetDieselPrice();
            }
            else
            {
                costPerMileFuel = EnCar[selectedEngineCar].GetTankSize() / EnCar[selectedEngineCar].GetRange() * FP.GetPetrolPrice();
            }
            costPerMileElectric = Math.Round(costPerMileElectric, 2);
            costPerMileFuel = Math.Round(costPerMileFuel, 2);
            string send = "It will cost you roughly " + costPerMileFuel + " pence per mile using " + EnCar[selectedEngineCar].GetName() + "\nwhereas it costs roughly "
                + costPerMileElectric + " pence per mile using " + ElCar[selectedElectricCar].GetName();
            return send;
        }
        public string JourneyEfficiency() //calculates journey effeciency by working out expected battery / fuel usage for the journey and comparing it to the actual efficiency the driver got
        {
            if (SettingsData.GetVehicleType() != "HYBRID") //as it is only for engine or electric vehicles
            {
                int distanceTravelled;
                int batteryUsed;
                int fuelUsed;
                double perfectEfficiency;
                double actualEfficiency;
                Console.WriteLine("How far did you travel in miles?");
                distanceTravelled = IntValidation(Console.ReadLine(), "Please enter how far you travelled in miles", 1, 800);
                switch (SettingsData.GetVehicleType())
                {
                    case "ENGINE":
                        Console.WriteLine("Roughly how much fuel did you use in litres");
                        fuelUsed = IntValidation(Console.ReadLine(), "Please enter a suitable value for your fuel consumption", 1, 128);
                        perfectEfficiency = EnCar[SettingsData.GetVehicleIndex()].GetRange() / EnCar[SettingsData.GetVehicleIndex()].GetTankSize();
                        actualEfficiency = distanceTravelled / fuelUsed;
                        if (actualEfficiency * 0.9 > perfectEfficiency)
                        {
                            return "Your journey was very efficient";
                        }
                        if (actualEfficiency > perfectEfficiency)
                        {
                            return "Your journey was efficienct";
                        }
                        if (actualEfficiency < perfectEfficiency)
                        {
                            return "Your journey was inefficient";
                        }
                        if (actualEfficiency < perfectEfficiency * 0.9)
                        {
                            return "Your journey was very inefficient";
                        }
                        return null;

                    case "ELECTRIC":
                        Console.WriteLine("Roughly what battery percent did you use?");
                        batteryUsed = IntValidation(Console.ReadLine(), "Please enter the battery percent you used for your journey", 1, 100);
                        perfectEfficiency = ElCar[SettingsData.GetVehicleIndex()].GetRange() / ElCar[SettingsData.GetVehicleIndex()].GetBatterySize(); //range per kWH
                        actualEfficiency = distanceTravelled / (ElCar[SettingsData.GetVehicleIndex()].GetBatterySize() / batteryUsed * 100);
                        if (actualEfficiency > perfectEfficiency)
                        {
                            return "Your journey was very efficient";
                        }
                        if (actualEfficiency >= perfectEfficiency)
                        {
                            return "Your journey was efficienct";
                        }
                        if (actualEfficiency < perfectEfficiency)
                        {
                            return "Your journey was inefficient";
                        }
                        else
                        {
                            return "Your journey was very inefficient";
                        }

                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }
        public void CreateVehicle()
        {
            int engineCarAmount = Convert.ToInt32(vehicleData[0]);
            int electricCarAmount = Convert.ToInt32(vehicleData[1]);
            int hybridCarAmount = Convert.ToInt32(vehicleData[2]);
            int engineDataLength = 5;
            int electricDataLength = 3;
            int hybridDataLength = 7;
            string[] uploadData = File.ReadAllLines(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data.txt");
            int dataLength = (engineCarAmount * engineDataLength) + (electricCarAmount * electricDataLength) + (hybridCarAmount * hybridDataLength);
            for (int i = 0; i < uploadData.Length; i++)
            {
                vehicleData[i] = uploadData[i];
            }
            int index;
            Console.WriteLine("Type a vehicle type: Engine, Electric or Hybrid");
            string vehicleType = Console.ReadLine();
            if (vehicleType.ToUpper().Contains("EN")) //collects all the vehicle data for that vehicle type except range and name as they are common of all vehicle types so are done seperately
            {
                index = 3; //after all the vehicle amount values
                for (int i = dataLength + 3; i > 0; i--) //shifts everything forward in the array
                {
                    vehicleData[i + engineDataLength] = vehicleData[i];
                }
                vehicleData[0] = Convert.ToString(Convert.ToInt32(vehicleData[0]) + 1);
                Console.WriteLine("Type a name for your vehicle");
                vehicleData[index] = Console.ReadLine();
                Console.WriteLine("Enter the range of your vehicle");
                vehicleData[index + 1] = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a realistic value for your vehicles range", 1, 1000));
                Console.WriteLine("Enter the tank size (L), road tax and fuel type (either P or D) for your vehicle one after another");
                vehicleData[index + 2] = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a realistic tank size for your vehicle", 1, 200));
                vehicleData[index + 3] = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a real road tax value for your vehicle", 0, 2135));
                vehicleData[index + 4] = Console.ReadLine().ToUpper();
                while (vehicleData[index + 4] != "P" && vehicleData[index + 4] != "D")
                {
                    Console.WriteLine("Enter a suitable fuel type, P for petrol and D for diesel");
                    vehicleData[index + 4] = Console.ReadLine();
                }
            }
            if (vehicleType.ToUpper().Contains("HY"))
            {
                index = 3 + (electricDataLength * electricCarAmount) + (engineDataLength * engineCarAmount); //so it knows where in the array to place it
                for (int i = dataLength + 3; i > index - hybridDataLength; i--) //shifts everything forward in the array
                {
                    vehicleData[i + hybridDataLength] = vehicleData[i];
                }
                vehicleData[2] = Convert.ToString(Convert.ToInt32(vehicleData[2]) + 1);
                Console.WriteLine("Type a name for your vehicle");
                vehicleData[index] = Console.ReadLine();
                Console.WriteLine("Enter the range of your vehicle");
                vehicleData[index + 1] = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a realistic value for your vehicles range", 1, 1000));
                Console.WriteLine("Enter the tank size in litres (L), battery size in kWh, fuel type (either P or D), the electric range and engine range in miles");
                vehicleData[index + 2] = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a realistic tank size for your vehicle", 1, 200));
                vehicleData[index + 3] = Convert.ToString(IntValidation(Console.ReadLine(), "Enter a realistic battery size value in kWh", 1, 110));
                vehicleData[index + 4] = Console.ReadLine().ToUpper();
                while (vehicleData[index + 4] != "P" && vehicleData[index + 4] != "D")
                {
                    Console.WriteLine("Enter a suitable fuel type, P for petrol and D for diesel");
                    vehicleData[index + 4] = Console.ReadLine();
                }
                vehicleData[index + 5] = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a realistic value for your vehicles range", 1, 500));
                vehicleData[index + 6] = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a realistic value for your vehicles range", 1, 1000));
            }
            if (vehicleType.ToUpper().Contains("EL"))
            {
                index = 3 + (engineDataLength * engineCarAmount);
                for (int i = dataLength + 3; i > index - electricDataLength; i--) //shifts everything forward in the array
                {
                    vehicleData[i + electricDataLength] = vehicleData[i];
                }
                vehicleData[1] = Convert.ToString(Convert.ToInt32(vehicleData[1]) + 1);
                Console.WriteLine("Type a name for your vehicle");
                vehicleData[index] = Console.ReadLine();
                Console.WriteLine("Enter the range of your vehicle");
                vehicleData[index + 1] = Convert.ToString(IntValidation(Console.ReadLine(), "Please enter a realistic value for your vehicles range", 1, 1000));
                Console.WriteLine("Enter the battery size of your vehicle in kWh");
                vehicleData[index + 2] = Convert.ToString(IntValidation(Console.ReadLine(), "Enter a realistic battery size value in kWh", 1, 110));
            }
            File.WriteAllLines(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data.txt", vehicleData);
            LoadVehicleData();
        } //allows the user to create a vehicle to simulate
        public void RestoreToDefault() //replaces all the used text files with the resotre copies
        {
            File.Delete(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data.txt");
            File.Delete(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Settings.txt");
            File.Copy(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data restore.txt", @"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data.txt");
            File.Copy(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Settings restore.txt", @"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Settings.txt");
            LoadSettings();
            LoadVehicleData();
        }
        public void UpdateRestore() //replaces the restore file to the current vehicleData.txt file
        {
            Console.WriteLine("Are youy sure you want to update the restore file, this cannot be undone? (Y / N)");
            if (Console.ReadLine().ToUpper().Contains("Y"))
            {
                File.Delete(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data restore.txt");
                File.Copy(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data.txt", @"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data restore.txt");
                Console.WriteLine("Your restore file has now been updated to support any vehicles added this session");
            }
            else
            {
                Console.WriteLine("Operation cancelled");
            }
        }
        public void SortVehicleData(int enCarAmount, int elCarAmount, int hyCarAmount) //bubble sort as there are too little vlaues for a merge sort to be efficient, where 5, 3 and 7 are used, it is the length of the vehicle types data
        {
            string vehicleName = "";
            if (SettingsData.GetVehicleType() == "ENGINE") //this is because after sorting vehicle data the indexes of the vehicles change so need to be able to update default vehicle with the updated index
            {
                vehicleName = EnCar[SettingsData.GetVehicleIndex()].GetName();
            }
            if (SettingsData.GetVehicleType() == "ELECTRIC")
            {
                vehicleName = ElCar[SettingsData.GetVehicleIndex()].GetName();
            }
            if (SettingsData.GetVehicleType() == "HYBRID")
            {
                vehicleName = HyCar[SettingsData.GetVehicleIndex()].GetName();
            }
            bool swap = true;
            string[] temp = new string[7];

            while (swap == true)
            {
                int count;
                swap = false;
                for (int index = 4; index < 3 + (enCarAmount - 1) * 5; index += 5)
                {
                    if (Convert.ToInt32(vehicleData[index]) > Convert.ToInt32(vehicleData[index + 5])) //if the first vehicle range is larger than the second
                    {
                        count = index - 1;
                        for (int i = 0; i < 5; i++) //because it has to swap all the vehicle data, not just range
                        {
                            temp[i] = vehicleData[count]; //stores the 1st vehicle info starting with name hence -1 as the vehicle name is an index above range, then swaps them
                            vehicleData[count] = vehicleData[count + 5];
                            vehicleData[count + 5] = temp[i];
                            count++;
                        }
                        swap = true;
                    }
                }
                for (int index = 4 + enCarAmount * 5; index < 4 + (enCarAmount * 5) + ((elCarAmount - 1) * 3) - 1; index += 3) // -1 to use the name to start the switching opposed to range
                {
                    if (Convert.ToInt32(vehicleData[index]) > Convert.ToInt32(vehicleData[index + 3]))
                    {
                        count = index - 1;
                        for (int i = 0; i < 3; i++)
                        {
                            temp[i] = vehicleData[count];
                            vehicleData[count] = vehicleData[count + 3];
                            vehicleData[count + 3] = temp[i];
                            count++;
                        }
                    }
                }
                for (int index = 4 + (enCarAmount * 5) + (elCarAmount * 3); index < 4 + (enCarAmount * 5) + (elCarAmount * 3) + ((hyCarAmount - 1) * 7) - 1; index += 7)
                {
                    if (Convert.ToInt32(vehicleData[index]) > Convert.ToInt32(vehicleData[index + 7]))
                    {
                        count = index - 1;
                        for (int i = 0; i < 7; i++)
                        {
                            temp[i] = vehicleData[count];
                            vehicleData[count] = vehicleData[count + 7];
                            vehicleData[count + 7] = temp[i];
                            count++;
                        }
                    }
                }
            }
            File.WriteAllLines(@"E:\USB Backups\USB Backup 07.07.2020\College Y2\Computer Science\Coursework\Vehicle data.txt", vehicleData);
            LoadVehicleData();
            if (SettingsData.GetVehicleType() == "ENGINE") //works out the default index from the name calculated before the sort
            {
                for (int i = 0; i < enCarAmount; i++)
                {
                    if (EnCar[i].GetName() == vehicleName)
                    {
                        SettingsData.SetVehicleIndex(i);
                    }
                }
            }
            if (SettingsData.GetVehicleType() == "ELECTRIC")
            {
                for (int i = 0; i < elCarAmount; i++)
                {
                    if (ElCar[i].GetName() == vehicleName)
                    {
                        SettingsData.SetVehicleIndex(i);
                    }
                }
            }
            if (SettingsData.GetVehicleType() == "HYBRID")
            {
                for (int i = 0; i < hyCarAmount; i++)
                {
                    if (HyCar[i].GetName() == vehicleName)
                    {
                        SettingsData.SetVehicleIndex(i);
                    }
                }
            }
            //saves vehicle data and settings, then reloads them
            SaveSettings(SettingsData.GetWeatherIndex(), SettingsData.GetVehicleType(), SettingsData.GetVehicleIndex());
            LoadSettings();
            LoadVehicleData();
        }
        public int IntValidation(string data, string exceptionErrorMSG, int min, int max)
        {
            bool valid = false;
            int convertTo = -1;
            while (valid != true)
            {
                try
                {
                    convertTo = Convert.ToInt32(data);
                    if (convertTo <= max && convertTo >= min)
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        if (SettingsData.GetVehicleType() == "HYBRID" && max != 100)
                        {
                            Console.WriteLine(exceptionErrorMSG + ", the tank size for this vehicle is " + HyCar[SettingsData.GetVehicleIndex()].GetTankSize());
                        }
                        if (SettingsData.GetVehicleType() == "ENGINE")
                        {
                            Console.WriteLine(exceptionErrorMSG + ", the tank size for this vehicle is " + EnCar[SettingsData.GetVehicleIndex()].GetTankSize());
                        }
                        if (SettingsData.GetVehicleType() == "ELECTRIC")
                        {
                            Console.WriteLine(exceptionErrorMSG);
                        }
                        Console.ResetColor();
                        data = Console.ReadLine();
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(exceptionErrorMSG);
                    Console.ResetColor();
                    data = Console.ReadLine();
                }
            }
            return convertTo;
        }
    }
}
