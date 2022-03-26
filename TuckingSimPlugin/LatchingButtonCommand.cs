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

            foreach (var button in TruckingSimPlugin.Configuration.Buttons
                .Where(b => b.Style == Common.Configuration.ButtonConfiguration.ButtonStyle.LatchingButton))
            {
                // Add a Moment Button
                this.AddParameter(
                    button.SafeName,
                    button.FullName,
                    button.GroupName,
                    button.Style.ToString(),
                    LoupedeckOperatingSystem.Win);

                Telemetry.Add(button.SafeName, button.DefaultValue);

                // Skip adding Telemetry Subscriber if there's no Telemetry Item
                if (button.TelemetryItem == null) continue;

                TruckingSimPlugin.Telemetry
                    .Select(data => {
                        return resolver.ResolveSafe(data, button.TelemetryItem);
                    })
                    .DistinctUntilChanged()
                    .Subscribe(itemValue => {
                        this.Telemetry[button.SafeName] = itemValue;
                        this.ActionImageChanged(button.SafeName);
                    });
            }
        }

        protected override void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var buttons = TruckingSimPlugin.Configuration.Buttons;
            var button = buttons.Where(b => b.SafeName == actionParameter).First();

            Boolean state = false;

            if (button.TelemetryItem == null)
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
            SetButtonState(joyId, button.ButtonId, state);

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
            return TruckingSimPlugin.Configuration.Buttons.Where(i => i.SafeName == safeName).First();
        }

    }
}
