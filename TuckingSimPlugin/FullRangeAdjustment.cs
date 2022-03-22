namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FullRangeAdjustment : FullRangeAdjustmentBase
    {
        private Dictionary<String, Int32> AdjustmentValues = new Dictionary<String, Int32>();

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
                AdjustmentValues.Add(adjuster.SafeName, adjuster.DefaultValue);
            }
        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var adjusters = TruckingSimPlugin.Configuration.FullRangeAdjustments;
            var adjuster = adjusters.Find(d => d.SafeName == actionParameter);
            
            // Reset to default value on press
            AdjustmentValues[actionParameter] = adjuster.DefaultValue;

            // Set Axis
            SetAxis(joyId, adjuster.Axis, AdjustmentValues[actionParameter]);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var adjusters = TruckingSimPlugin.Configuration.FullRangeAdjustments;
            var adjuster = adjusters.Find(d => d.SafeName == actionParameter);

            // Increment
            var newValue = AdjustmentValues[actionParameter];
            newValue += TruckingSimPlugin.Configuration.JoystickAxisIncrementValue * ticks;
            if (newValue > TruckingSimPlugin.Configuration.JoystickAxisMaxValue)
                newValue = TruckingSimPlugin.Configuration.JoystickAxisMaxValue;
            if (newValue < TruckingSimPlugin.Configuration.JoystickAxisMinValue)
                newValue = TruckingSimPlugin.Configuration.JoystickAxisMinValue;
            AdjustmentValues[actionParameter] = newValue;

            // Set Axis
            SetAxis(joyId, adjuster.Axis, AdjustmentValues[actionParameter]);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {

            if (actionParameter == null)
                return String.Empty;

            var adjusters = TruckingSimPlugin.Configuration.FullRangeAdjustments;
            var adjuster = adjusters.Where(a => a.SafeName == actionParameter).First();
            if (adjuster == null)
            {
                return "actionParameter";
            }

            var currVal = AdjustmentValues[actionParameter];
            return String.Format(adjuster.DisplayText, currVal);
        }

    }
}
