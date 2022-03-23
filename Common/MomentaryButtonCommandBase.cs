namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public abstract class MomentaryButtonCommandBase : PluginDynamicCommand
    {
        public MomentaryButtonCommandBase() : base()
        {
        }

        protected static async Task SendButtonPress(UInt32 vjoy, UInt32 vbutton, Int32 duration)
        {
            // Set button on for duration, then back off
            Observable
                .Return(VirtualJoystick.SendButtonPress(vjoy, vbutton, true))
                .Delay(new TimeSpan(0, 0, 0, 0, duration))
                .Subscribe(_ => VirtualJoystick.SendButtonPress(vjoy, vbutton, false));
        }
    }
}
