using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleDB.Vehicle
{
    [Serializable]
    public sealed class Motorcycle : VehicleBase, IRidable, IFlyable
    {
        public Motorcycle()
            : this(2)
        { 
        }

        public Motorcycle(ushort wheels)
            : base()
        {
            this.Wheels = wheels;
            this.Drive = DRIVE.REAR_WHEEL;
        }

        public override string GetDescription()
        {
            return "   \\m/\n" + base.GetDescription();
        }

        public void DriveTo(string loc)
        {
            Console.WriteLine("The \"" + this.Type + "\" is driving to " + loc + ".");
        }

        public void FlyTo(string loc)
        {
            Console.WriteLine("The \"" + this.Type + "\" is flying to " + loc + ".");
        }
    }
}
