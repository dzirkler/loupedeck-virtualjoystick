namespace DesertSunSoftware.LoupedeckVirtualJoystick.BeamNgDrivePlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public class BeamNgDriveApplication : ClientApplication
    {
        public BeamNgDriveApplication()
        {
        }

        protected override String[] GetProcessNames() => new[] { "BeamNG.drive" };

        protected override String GetBundleName() => "";


    }
}