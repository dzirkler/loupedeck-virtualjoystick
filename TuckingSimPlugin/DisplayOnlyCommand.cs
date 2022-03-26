namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
    using Loupedeck;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using Pather.CSharp;
    using SCSSdkClient.Object;

    public class DisplayOnlyCommand : DisplayOnlyCommandBase
    {
        private Dictionary<String, Object> Telemetry = new Dictionary<String, Object>();

        public DisplayOnlyCommand() : base()
        {
            var resolver = new Resolver();

            TruckingSimPlugin
                .Configuration
                .Items
                .Where(i => i is ButtonConfiguration)
                .Cast<ButtonConfiguration>()
                .Where(i => i.Style == ButtonConfiguration.ButtonStyle.DisplayButton)
                .ToList()
                .ForEach(item =>
                {
                    // Add a Display Button
                    this.AddParameter(
                        item.SafeName,
                        item.FullName,
                        item.GroupName,
                        "Display",
                        LoupedeckOperatingSystem.Win);

                    // Seed Storage
                    Telemetry.Add(item.SafeName, true);

                    // Wire Telemetry Watcher
                    if (item.TelemetryItem != null && item.TelemetryItem != String.Empty)
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
            // No Op - But Loupedeck requires the function or it won't show up!
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

        private ButtonConfiguration GetConfigItem(String safeName)
        {
            return TruckingSimPlugin
                .Configuration
                .Items
                .Where(b => b.SafeName == safeName)
                .Cast<ButtonConfiguration>()
                .First();
        }

    }
}
