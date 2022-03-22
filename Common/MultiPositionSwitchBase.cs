namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public abstract class MultiPositionSwitchBase : PluginDynamicAdjustment
    {
        public MultiPositionSwitchBase(Boolean hasReset) : base(hasReset)
        {
        }

        protected static void SetButtonState(UInt32 vjoy, UInt32 vbutton, Boolean state)
        {
            // Set Button State
            VirtualJoystick.SendButtonPress(vjoy, vbutton, state);
        }
    }
}
