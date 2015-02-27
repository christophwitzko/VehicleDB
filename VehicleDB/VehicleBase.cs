using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleDB
{
    [Serializable]
    public abstract class VehicleBase: IVehicle
    {
        private ushort wheels = 0;
        private double price = 0;
        private string brand = "Unknown";
        private string type = "Unknown";
        private DRIVE drive = DRIVE.NO_WHEEL;
        private double fuelconsumption = 0;
        private string id;

        public ushort Wheels
        {
            get { return this.wheels; }
            set 
            {
                if (value > 10) throw new Exception("Too much wheels!");
                this.wheels = value;
            }
        }
        public double Price
        {
            get { return this.price; }
            set 
            {
                if (value < 0) throw new Exception("Price must be bigger than 0!");
                this.price = value; 
            }
        }
        public string Brand
        {
            get { return this.brand; }
            set { this.brand = value; }
        }

        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public DRIVE Drive
        {
            get { return this.drive; }
            set { this.drive = value; }
        }

        public double FuelConsumption
        {
            get { return this.fuelconsumption; }
            set 
            {
                if (value < 0) throw new Exception("FuelConsumption must be bigger than 0!");
                this.fuelconsumption = value; 
            }
        }

        public string _ID
        {
            get 
            {
                return this.id;
            }
            private set 
            {
                this.id = value;
            }
        
        }

        public VehicleBase()
        {
            this._ID = Guid.NewGuid().ToString("D");
        }

        public virtual string GetDescription()
        {
            return string.Format("Vehicle: {0}\nID: {1}\nWheels: {2}\nPrice: {3:#,##0.00} EUR\nType (Brand): {4} ({5})\nDrive: {6}\nFuel-Consumption: {7:0.000}", this.GetType().Name, this._ID, this.Wheels, this.Price, this.Type, this.Brand, this.Drive, this.FuelConsumption);
        }

        public virtual void PrintDescription()
        {
            Console.WriteLine(this.GetDescription());
        }

        public virtual bool GetUserInput()
        {
            try
            {
                Console.Write("Price (EUR): ");
                this.Price = double.Parse(Console.ReadLine());
                Console.Write("Brand: ");
                this.Brand = Console.ReadLine();
                Console.Write("Type: ");
                this.Type = Console.ReadLine();
                Console.Write("Drive (ALL_WHEEL(1), REAR_WHEEL(2), FRONT_WHEEL(3), NO_WHEEL(4), JET(5)): ");
                int dinp = int.Parse(Console.ReadLine());
                if (dinp < 1 || dinp > 5) throw new Exception("Drive not available!");
                this.Drive = (DRIVE)dinp;
                Console.Write("Fuel-Consumption (l/100km): ");
                this.FuelConsumption = double.Parse(Console.ReadLine());
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR]: " + e.Message);
                return false;
            }
        }
    }
}
