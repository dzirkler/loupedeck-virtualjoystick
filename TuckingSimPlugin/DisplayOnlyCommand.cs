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
    using SCSSdkClient.Object;

    public class DisplayOnlyCommand : DisplayOnlyCommandBase
    {
        private bool SDKRunning = true;
        private bool GamePaused = true;

        public DisplayOnlyCommand() : base()
        {
            foreach (var momentButton in TruckingSimPlugin.Configuration.Buttons
                .Where(b => b.Style == Common.Configuration.ButtonConfiguration.ButtonStyle.DisplayButton))
            {
                // Add a Display Button
                this.AddParameter(
                    momentButton.SafeName,
                    momentButton.FullName,
                    momentButton.GroupName,
                    "Display",
                    LoupedeckOperatingSystem.Win);
            }

            TruckingSimPlugin.Telemetry
                .Select(data => data.Paused)
                .DistinctUntilChanged()
                .Subscribe(paused => {
                    GamePaused = paused;
                    this.ActionImageChanged("GameInfo-GamePaused");
                    });

            TruckingSimPlugin.Telemetry
                .Select(data => data.SdkActive)
                .DistinctUntilChanged()
                .Subscribe(running => {
                    SDKRunning = running;
                    this.ActionImageChanged("GameInfo-GameActive");
                });
        }

        protected override async void RunCommand(String actionParameter)
        {
            // No Op - But Loupedeck requires the function or it won't show up!
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            string yesNo = "No";
            switch (actionParameter)
            {
                case "GameInfo-GameActive":
                    yesNo = SDKRunning ? "Yes" : "No";
                    break;
                case "GameInfo-GamePaused":
                    yesNo = GamePaused ? "Yes" : "No";
                    break;
                default:
                    break;
            }

            
            var button = TruckingSimPlugin.Configuration.Buttons.Find(b => b.SafeName == actionParameter);
            return button == null ? "actionParameter" : String.Format(button.DisplayText, yesNo);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            return actionParameter.GetIconImage(GetCommandDisplayName(actionParameter, imageSize));
        }
    }
}
