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
            TruckingSimPlugin
                .Configuration
                .Items
                .Where(i => i is MultiPositionSwitchConfiguration)
                .Cast<MultiPositionSwitchConfiguration>()
                .ToList()
                .ForEach(item =>
                {
                    this.AddParameter(
                        item.SafeName,
                        item.FullName,
                        item.GroupName,
                        "Multi-Position Switch",
                        LoupedeckOperatingSystem.Win);

                    Telemetry.Add(item.SafeName, item.DefaultValue);
                });
        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var item = GetConfigItem(actionParameter);

            // Reset to default value on press
            Telemetry[actionParameter] = item.DefaultValue;

            //Set vJoy State
            foreach (var position in item.PositionValues)
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
            var item = GetConfigItem(actionParameter);

            // Increment
            var newValue = Telemetry[actionParameter];
            newValue += ticks;
            if (newValue > item.Positions)
                newValue = item.WrapAround ? 1 : item.Positions;
            if (newValue < 1)
                newValue = item.WrapAround ? item.Positions : 1;
            Telemetry[actionParameter] = newValue;

            //Set vJoy State
            foreach (var position in item.PositionValues)
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

        private MultiPositionSwitchConfiguration GetConfigItem(String safeName)
        {
            return TruckingSimPlugin
                .Configuration
                .Items
                .Where(i => i.SafeName == safeName)
                .Cast<MultiPositionSwitchConfiguration>()
                .FirstOrDefault();
        }

    }
}
