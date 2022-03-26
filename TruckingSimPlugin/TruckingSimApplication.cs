namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public class TruckingSimApplication : ClientApplication
    {
        public TruckingSimApplication()
        {
        }

        protected override String[] GetProcessNames() => new[] { "American Truck Simulator", "amtrucks" };

        protected override String GetBundleName() => "";


    }
}