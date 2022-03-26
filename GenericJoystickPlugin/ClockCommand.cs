namespace DesertSunSoftware.LoupedeckVirtualJoystick.GenericJoystickPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
    using Loupedeck;

    public class ClockCommand : PluginDynamicCommand
    {
        public ClockCommand() : base("Clock", "Displays a clock on a simple black background.", "Clock", DeviceType.All)
        {
            Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => this.ActionImageChanged());
        }

        protected override void RunCommand(String actionParameter)
        {
            // No Op - But Loupedeck requires the function or it won't show up!
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            this.ActionImageChanged();
            return "Clock";
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var iconFile = EmbeddedResources.FindFile("Blank.png");
            // Text Only
            using (var bitmapBuilder = new BitmapBuilder(80, 80))
            {
                bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(iconFile));
                bitmapBuilder.DrawText(DateTime.Now.ToString());

                return bitmapBuilder.ToImage();
            }
        }


    }
}
