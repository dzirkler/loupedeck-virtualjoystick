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

            // Init vJoy Wrapper & Query Capabilities
            var initSuccess = VirtualJoystick.Init();
            if (!initSuccess)
            {
                throw new InvalidOperationException("Cannot connect to vJoy. Please confirm it is running and restart Loupedeck software.");
            }
        }

        protected override String GetProcessName() => "";

        protected override String GetBundleName() => "";


    }
}