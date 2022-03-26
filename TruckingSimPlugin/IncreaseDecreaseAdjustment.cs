namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
    using Loupedeck;
    using Pather.CSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IncreaseDecreaseAdjustment : IncreaseDecreaseAdjustmentBase
    {
        private Dictionary<String, Object> Telemetry = new Dictionary<String, Object>();

        public IncreaseDecreaseAdjustment() : base(true)
        {
            var resolver = new Resolver();

            TruckingSimPlugin
                .Configuration
                .Items
                .Where(i => i is IncreaseDecreaseAdjustmentConfiguration)
                .Cast<IncreaseDecreaseAdjustmentConfiguration>()
                .ToList()
                .ForEach(item =>
                {
                    // Add a Display Button
                    this.AddParameter(
                        item.SafeName,
                        item.FullName,
                        item.GroupName,
                        "IncreaseDecrease",
                        LoupedeckOperatingSystem.Win);


                    // Seed Storage
                    Telemetry.Add(item.SafeName, true);

                    if (item.TelemetryItem != null && item.TelemetryItem != String.Empty)
                        // Wire Telemetry Watcher
                        TruckingSimPlugin.Telemetry
                                .Where(i => i.Item == item.TelemetryItem)
                                .Subscribe(telemetryItem =>
                                {
                                    this.Telemetry[item.SafeName] = telemetryItem.Value;
                                    this.ActionImageChanged(item.SafeName);
                                });
                });
        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var item = GetConfigItem(actionParameter);

            await SendButtonPress(joyId, item.ResetButtonId, TruckingSimPlugin.Configuration.MomentaryButtonDepressTime);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override async void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var item = GetConfigItem(actionParameter);

            // Increase or Decrease?
            var buttonId = ticks > 0 ? item.IncreaseButtonId : item.DecreaseButtonId;

            await SendButtonPress(joyId, buttonId, TruckingSimPlugin.Configuration.MomentaryButtonDepressTime);

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

        private IncreaseDecreaseAdjustmentConfiguration GetConfigItem(String safeName)
        {
            return TruckingSimPlugin
                .Configuration
                .Items
                .Where(b => b.SafeName == safeName)
                .Cast<IncreaseDecreaseAdjustmentConfiguration>()
                .First();
        }
    }
}
