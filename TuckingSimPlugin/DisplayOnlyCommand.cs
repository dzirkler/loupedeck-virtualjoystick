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
    using Loupedeck;
    using Microsoft.CodeAnalysis.CSharp.Scripting;
    using SCSSdkClient.Object;

    public class DisplayOnlyCommand : DisplayOnlyCommandBase
    {
        private Dictionary<String, Object> ButtonStates = new Dictionary<String, Object>();

        //private bool SDKRunning = true;
        //private bool GamePaused = true;

        public DisplayOnlyCommand() : base()
        {
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
                ButtonStates.Add(button.SafeName, false);

                TruckingSimPlugin.Telemetry
                    .Select(data => {
                        return data.GetType().GetProperty(button.TelemetryItem).GetValue(data);
                    })
                    .DistinctUntilChanged()
                    .Subscribe(itemValue => {
                    this.ButtonStates[button.SafeName] = itemValue;
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

            var button = TruckingSimPlugin.Configuration.Buttons.Find(b => b.SafeName == actionParameter);
            return button == null ? "actionParameter" : String.Format(button.DisplayText, ButtonStates[actionParameter]);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return actionParameter.GetIconImage(GetCommandDisplayName(actionParameter, imageSize));
        }
    }
}
