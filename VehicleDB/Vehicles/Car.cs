using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleDB.Vehicle
{
    [Serializable]
    public sealed class Car: VehicleBase, IRidable
    {
        public Car()
            : base()
        {
            this.Wheels = 4;
            this.Drive = DRIVE.FRONT_WHEEL;
        }

        public void DriveTo(string loc)
        {
            Console.WriteLine("The \"" + this.Type + "\" is driving to " + loc + ".");
        }

        
    }
}
