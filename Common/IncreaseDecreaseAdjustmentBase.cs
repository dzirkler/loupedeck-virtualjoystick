namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public abstract class IncreaseDecreaseAdjustmentBase : PluginDynamicAdjustment
    {
        public IncreaseDecreaseAdjustmentBase(Boolean hasReset) : base(hasReset)
        {
        }

        protected static async Task SendButtonPress(UInt32 vjoy, UInt32 vbutton, Int32 duration)
        {
            // Turn on button
            VirtualJoystick.SendButtonPress(vjoy, vbutton, true);

            // Pause for a moment
            await Task.Delay(duration);

            // Turn off button
            VirtualJoystick.SendButtonPress(vjoy, vbutton, false);
        }
    }
}
