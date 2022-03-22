namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
    using Loupedeck;

    public abstract class FullRangeAdjustmentBase : PluginDynamicAdjustment
    {
        public FullRangeAdjustmentBase(Boolean hasReset) : base(hasReset)
        {
        }

        protected static void SetAxis(UInt32 vjoy, FullRangeAdjustmentConfiguration.JoystickAxis axis, Int32 value)
        {
            VirtualJoystick.SetAxis(vjoy, axis, value);
        }
    }
}
