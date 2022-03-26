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

    public class MultiPositionSwitch : MultiPositionSwitchBase
    {
        private Dictionary<String, Int32> Telemetry = new Dictionary<String, Int32>();

        public MultiPositionSwitch() : base(true)
        {

            foreach (var mpSwitch in TruckingSimPlugin.Configuration.MultiPositionSwitches)
            {
                // Add a Dial
                this.AddParameter(
                    mpSwitch.SafeName,
                    mpSwitch.FullName,
                    mpSwitch.GroupName,
                    "Multi-Position Switch",
                    LoupedeckOperatingSystem.Win);

                Telemetry.Add(mpSwitch.SafeName, mpSwitch.DefaultValue);
            }

        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var mpSwitches = TruckingSimPlugin.Configuration.MultiPositionSwitches;
            var mpSwitch = mpSwitches.Where(d => d.SafeName == actionParameter).First();
            
            // Reset to default value on press
            Telemetry[actionParameter] = mpSwitch.DefaultValue;

            //Set vJoy State
            foreach (var position in mpSwitch.PositionValues)
            {
                if (position.ButtonId != UInt32.MaxValue)
                    SetButtonState(joyId, position.ButtonId, Telemetry[actionParameter] == position.PositionId);
            }

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var mpSwitches = TruckingSimPlugin.Configuration.MultiPositionSwitches;
            var mpSwitch = mpSwitches.Where(d => d.SafeName == actionParameter).First();

            // Increment
            var newValue = Telemetry[actionParameter];
            newValue += ticks;
            if (newValue > mpSwitch.Positions)
                newValue = mpSwitch.WrapAround ? 1 : mpSwitch.Positions;
            if (newValue < 1)
                newValue = mpSwitch.WrapAround ? mpSwitch.Positions : 1;
            Telemetry[actionParameter] = newValue;

            //Set vJoy State
            foreach (var position in mpSwitch.PositionValues)
            {
                if (position.ButtonId != UInt32.MaxValue)
                    SetButtonState(joyId, position.ButtonId, Telemetry[actionParameter] == position.PositionId);
            }

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
            return TruckingSimPlugin.Configuration.MultiPositionSwitches.Where(i => i.SafeName == safeName).First();
        }

    }
}
