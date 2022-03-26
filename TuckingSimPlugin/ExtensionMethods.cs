namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using Loupedeck;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal static class ExtensionMethods
    {

        public static BitmapImage GetIconImage(this String safeName, string displayText)
        {
            return GetIconImage(safeName, displayText, "On");
        }

        public static BitmapImage GetIconImage(this String safeName, String displayText, Object telemetry)
        {
            if (telemetry != null && telemetry is Boolean)
            {
                return GetIconImage(safeName, displayText, (Boolean)telemetry ? "On" : "Off");
            }
            else
            {
                return GetIconImage(safeName, displayText);
            }
        }

        public static BitmapImage GetIconImage(this String safeName, String displayText, String state)
        {
            if (safeName == null) return GetBlankImage(displayText);

            var iconFile = EmbeddedResources.FindFile($"{safeName}-{state}.png");
            if (iconFile == null) return GetBlankImage(displayText);

            return EmbeddedResources.ReadImage(iconFile);
        }

        private static BitmapImage GetBlankImage(String displayText)
        {
            return GetBlankImage(displayText, "");
        }

        private static BitmapImage GetBlankImage(String displayText, String state)
        {
            var iconFile = EmbeddedResources.FindFile("Blank.png");
            // Text Only
            using (var bitmapBuilder = new BitmapBuilder(80,80))
            {
                bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(iconFile));
                bitmapBuilder.DrawText(String.Format(displayText, state));

                return bitmapBuilder.ToImage();
            }
        }

    }
}
