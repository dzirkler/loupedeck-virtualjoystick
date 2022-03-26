namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
    using Loupedeck;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FullRangeAdjustment : FullRangeAdjustmentBase
    {
        private Dictionary<String, Int32> Telemetry = new Dictionary<String, Int32>();

        public FullRangeAdjustment() : base(true)
        {
            foreach (var adjuster in TruckingSimPlugin.Configuration.FullRangeAdjustments)
            {
                // Add a Dial
                this.AddParameter(
                    adjuster.SafeName,
                    adjuster.FullName,
                    adjuster.GroupName,
                    "Dial",
                    LoupedeckOperatingSystem.Win);

                // Set Current Val
                Telemetry.Add(adjuster.SafeName, adjuster.DefaultValue);
            }
        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var adjusters = TruckingSimPlugin.Configuration.FullRangeAdjustments;
            var adjuster = adjusters.Find(d => d.SafeName == actionParameter);
            
            // Reset to default value on press
            Telemetry[actionParameter] = adjuster.DefaultValue;

            // Set Axis
            SetAxis(joyId, adjuster.Axis, Telemetry[actionParameter]);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var adjusters = TruckingSimPlugin.Configuration.FullRangeAdjustments;
            var adjuster = adjusters.Find(d => d.SafeName == actionParameter);

            // Increment
            var newValue = Telemetry[actionParameter];
            newValue += TruckingSimPlugin.Configuration.JoystickAxisIncrementValue * ticks;
            if (newValue > TruckingSimPlugin.Configuration.JoystickAxisMaxValue)
                newValue = TruckingSimPlugin.Configuration.JoystickAxisMaxValue;
            if (newValue < TruckingSimPlugin.Configuration.JoystickAxisMinValue)
                newValue = TruckingSimPlugin.Configuration.JoystickAxisMinValue;
            Telemetry[actionParameter] = newValue;

            // Set Axis
            SetAxis(joyId, adjuster.Axis, Telemetry[actionParameter]);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter == null || actionParameter == "") return null;

            return GetConfigItem(actionParameter).FormatCommandText(Telemetry[actionParameter]);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return actionParameter.GetIconImage(GetConfigItem(actionParameter).FormatIconText(Telemetry[actionParameter]), Telemetry[actionParameter]);
        }

        private IConfigurationItem GetConfigItem(String safeName)
        {
            return TruckingSimPlugin.Configuration.FullRangeAdjustments.Where(i => i.SafeName == safeName).First();
        }

    }
}
