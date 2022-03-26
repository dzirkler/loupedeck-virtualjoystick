namespace DesertSunSoftware.LoupedeckVirtualJoystick.BeamNgDrivePlugin
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

            BeamNgDrivePlugin
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

                    Telemetry.Add(item.SafeName, true);
                });
        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = BeamNgDrivePlugin.Configuration.vJoyID;
            var item = GetConfigItem(actionParameter);

            await SendButtonPress(joyId, item.ResetButtonId, BeamNgDrivePlugin.Configuration.MomentaryButtonDepressTime);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override async void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var joyId = BeamNgDrivePlugin.Configuration.vJoyID;
            var item = GetConfigItem(actionParameter);

            // Increase or Decrease?
            var buttonId = ticks > 0 ? item.IncreaseButtonId : item.DecreaseButtonId;

            await SendButtonPress(joyId, buttonId, BeamNgDrivePlugin.Configuration.MomentaryButtonDepressTime);

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
            return BeamNgDrivePlugin
                .Configuration
                .Items
                .Where(b => b.SafeName == safeName)
                .Cast<IncreaseDecreaseAdjustmentConfiguration>()
                .First();
        }
    }
}
