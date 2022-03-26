﻿namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
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

            TruckingSimPlugin
                .Configuration
                .Items
                .Where(i => i is FullRangeAdjustmentConfiguration)
                .Cast<FullRangeAdjustmentConfiguration>()
                .ToList()
                .ForEach(item =>
                {
                                // Add a Display Button
                                this.AddParameter(
                        item.SafeName,
                        item.FullName,
                        item.GroupName,
                        "Dial",
                        LoupedeckOperatingSystem.Win);

                        // Seed Storage
                        Telemetry.Add(item.SafeName, item.DefaultValue);

                });
        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var item = GetConfigItem(actionParameter);

            // Reset to default value on press
            Telemetry[actionParameter] = item.DefaultValue;

            // Set Axis
            SetAxis(joyId, item.Axis, Telemetry[actionParameter]);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;

            // Increment
            var newValue = Telemetry[actionParameter];
            newValue += TruckingSimPlugin.Configuration.JoystickAxisIncrementValue * ticks;
            if (newValue > TruckingSimPlugin.Configuration.JoystickAxisMaxValue)
                newValue = TruckingSimPlugin.Configuration.JoystickAxisMaxValue;
            if (newValue < TruckingSimPlugin.Configuration.JoystickAxisMinValue)
                newValue = TruckingSimPlugin.Configuration.JoystickAxisMinValue;
            Telemetry[actionParameter] = newValue;

            // Set Axis
            SetAxis(joyId, GetConfigItem(actionParameter).Axis, Telemetry[actionParameter]);

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

        private FullRangeAdjustmentConfiguration GetConfigItem(String safeName)
        {
            return TruckingSimPlugin
                .Configuration
                .Items
                .Where(b => b.SafeName == safeName)
                .Cast<FullRangeAdjustmentConfiguration>()
                .First();
        }

    }
}
