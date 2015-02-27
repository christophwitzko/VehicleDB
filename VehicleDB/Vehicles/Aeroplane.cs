using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleDB.Vehicle
{
    [Serializable]
    public sealed class Aeroplane: VehicleBase, IFlyable
    {
        public Aeroplane()
            : base()
        {
            this.Wheels = 0;
            this.Drive = DRIVE.NO_WHEEL;
        }

        public void FlyTo(string loc)
        {
            Console.WriteLine("The \"" + this.Type + "\" is flying to " + loc + ".");
        }

    }
}
