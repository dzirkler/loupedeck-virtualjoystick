namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public class LatchingButtonCommand : LatchingButtonCommandBase
    {
        private Dictionary<string, Boolean> ButtonStates = new Dictionary<string, Boolean>();

        public LatchingButtonCommand() : base()
        {
            foreach (var momentButton in TruckingSimPlugin.Configuration.Buttons
                .Where(b => b.Style == Common.Configuration.ButtonConfiguration.ButtonStyle.LatchingButton))
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

            Boolean state = false;

            if (ButtonStates.ContainsKey(actionParameter))
            {
                // Toggle State
                state = !ButtonStates[actionParameter];
                ButtonStates[actionParameter] = state;
            }
            else
            {
                ButtonStates.Add(actionParameter, true);
                state = true;
            }

            // Set Virtual Joystick Button
            SetButtonState(joyId, button.ButtonId, state);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter == null) return null;

            var button = TruckingSimPlugin.Configuration.Buttons.Find(mb => mb.SafeName == actionParameter);
            Boolean state = false;

            if (ButtonStates.ContainsKey(actionParameter))
            {
                state = ButtonStates[actionParameter];
            }
            else
            {
                ButtonStates.Add(actionParameter, false);
            }

            var onOff = state ? "on" : "off";

            return button == null ? "actionParameter" : String.Format(button.DisplayText, onOff);
        }
    }
}
