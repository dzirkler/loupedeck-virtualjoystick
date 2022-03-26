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

            foreach (var button in TruckingSimPlugin.Configuration.Buttons
                .Where(b => b.Style == Common.Configuration.ButtonConfiguration.ButtonStyle.DisplayButton))
            {
                // Add a Display Button
                this.AddParameter(
                    button.SafeName,
                    button.FullName,
                    button.GroupName,
                    "Display",
                    LoupedeckOperatingSystem.Win);

                // Seed Storage
                Telemetry.Add(button.SafeName, false);

                TruckingSimPlugin.Telemetry
                    .Select(data =>
                    {
                        return resolver.ResolveSafe(data, button.TelemetryItem);
                    })
                    .DistinctUntilChanged()
                    .Subscribe(itemValue =>
                    {
                        this.Telemetry[button.SafeName] = itemValue;
                        this.ActionImageChanged(button.SafeName);
                    });
            }
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

        private IConfigurationItem GetConfigItem(String safeName)
        {
            return TruckingSimPlugin.Configuration.Buttons.Where(b => b.SafeName == safeName).First();
        }

    }
}
