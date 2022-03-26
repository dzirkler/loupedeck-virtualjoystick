namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;
    using Pather.CSharp;

    public class MomentaryButtonCommand : MomentaryButtonCommandBase
    {
        private Dictionary<String, Object> Telemetry = new Dictionary<String, Object>();

        public MomentaryButtonCommand() : base()
        {
            var resolver = new Resolver();

            foreach (var button in TruckingSimPlugin.Configuration.Buttons
                .Where(b => b.Style == Common.Configuration.ButtonConfiguration.ButtonStyle.MomentaryButton))
            {
                // Add a Moment Button
                this.AddParameter(
                    button.SafeName,
                    button.FullName,
                    button.GroupName,
                    "Momentary",
                    LoupedeckOperatingSystem.Win);

                // Seed Storage
                Telemetry.Add(button.SafeName, false);

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

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var buttons = TruckingSimPlugin.Configuration.Buttons;
            var button = buttons.Where(b => b.SafeName == actionParameter).First();

            await SendButtonPress(joyId, button.ButtonId, TruckingSimPlugin.Configuration.MomentaryButtonDepressTime);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            var button = TruckingSimPlugin.Configuration.Buttons.Find(b => b.SafeName == actionParameter);
            return button == null ? "actionParameter" : button.DisplayText;
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var button = TruckingSimPlugin.Configuration.Buttons.Where(b => b.SafeName == actionParameter).First();
            if (button.TelemetryItem != null)
            {
                var onOff = (Boolean)Telemetry[actionParameter] ? "On" : "Off";
                return actionParameter.GetIconImage(GetCommandDisplayName(actionParameter, imageSize), onOff);
            }
            else
            {
                return actionParameter.GetIconImage(GetCommandDisplayName(actionParameter, imageSize));
            }
        }
    }
}
