namespace DesertSunSoftware.LoupedeckVirtualJoystick.GenericJoystickPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public class GenericJoystickApplication : ClientApplication
    {
        public GenericJoystickApplication()
        {
        }

        protected override String GetProcessName() => "";

        protected override String GetBundleName() => "";


    }
}