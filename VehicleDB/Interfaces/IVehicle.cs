using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleDB
{
    public enum DRIVE { ALL_WHEEL = 1, REAR_WHEEL = 2, FRONT_WHEEL = 3, NO_WHEEL = 4, JET = 5}
    public interface IVehicle
    {
        ushort Wheels { get; set; }
        double Price { get; set; }
        string Brand { get; set; }
        string Type { get; set; }
        DRIVE Drive { get; set; }
        double FuelConsumption { get; set; }
        string _ID { get; }
        string GetDescription();
        void PrintDescription();
        bool GetUserInput();
    }
}
