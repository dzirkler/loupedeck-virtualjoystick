namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public class MomentaryButtonCommand : MomentaryButtonCommandBase
    {
        public MomentaryButtonCommand() : base()
        {
            foreach (var momentButton in TruckingSimPlugin.Configuration.Buttons
                .Where(b => b.Style == Common.Configuration.ButtonConfiguration.ButtonStyle.MomentaryButton))
            {
                // Add a Moment Button
                this.AddParameter(
                    momentButton.SafeName,
                    momentButton.FullName,
                    momentButton.GroupName,
                    "Momentary",
                    LoupedeckOperatingSystem.Win);
            }
        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var buttons = TruckingSimPlugin.Configuration.Buttons;
            var button = buttons.Where(b => b.SafeName == actionParameter).First();

            await SendButtonPress(joyId, button.ButtonId, 100);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            var button = TruckingSimPlugin.Configuration.Buttons.Find(mb => mb.SafeName == actionParameter);
            return button == null ? "actionParameter" : button.DisplayText;
        }

    }
}
