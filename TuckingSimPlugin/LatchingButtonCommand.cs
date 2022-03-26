namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
    using Loupedeck;
    using Pather.CSharp;

    public class LatchingButtonCommand : LatchingButtonCommandBase
    {
        private Dictionary<String, Object> Telemetry = new Dictionary<String, Object>();

        public LatchingButtonCommand() : base()
        {
            var resolver = new Resolver();

            TruckingSimPlugin
                .Configuration
                .Items
                .Where(i => i is ButtonConfiguration)
                .Cast<ButtonConfiguration>()
                .Where(i => i.Style == ButtonConfiguration.ButtonStyle.LatchingButton)
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
                    Telemetry.Add(item.SafeName, false);

                    // Wire Telemetry Watcher
                    TruckingSimPlugin.Telemetry
                        .Select(data =>
                        {
                            return resolver.ResolveSafe(data, item.TelemetryItem);
                        })
                        .DistinctUntilChanged()
                        .Subscribe(itemValue =>
                        {
                            this.Telemetry[item.SafeName] = itemValue;
                            this.ActionImageChanged(item.SafeName);
                        });
                });
        }

        protected override void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var item = GetConfigItem(actionParameter);

            Boolean state = false;

            if (item.TelemetryItem == null)
            {
                // Only explictly Toggle state if we're not relying on Telemetry 

                if (Telemetry.ContainsKey(actionParameter))
                {
                    // Toggle State
                    state = !(Boolean)Telemetry[actionParameter];
                    Telemetry[actionParameter] = state;
                }
                else
                {
                    Telemetry.Add(actionParameter, true);
                    state = true;
                }
            }

            // Set Virtual Joystick Button
            SetButtonState(joyId, item.ButtonId, state);

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
