namespace DesertSunSoftware.LoupedeckVirtualJoystick.GenericJoystickPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public class GenericLatchingButtonCommand : LatchingButtonCommandBase
    {
        private Dictionary<string, Boolean> ButtonStates = new Dictionary<string, Boolean>();

        public GenericLatchingButtonCommand() : base("Latching Button", "Simulates holding a joystick button until another press releases it.", "Latching Button")
        {
            this.MakeProfileAction("tree");
            //this.DisplayName = "Latching Button";
            //this.Description = "Simulates holding a joystick button until another press releases it.";
        }

        protected override PluginProfileActionData GetProfileActionData()
        {
            var tree = new PluginProfileActionTree("Choose Virtual Joystick and Virtual Button LATCH");
            tree.AddLevel("Virtual Joystick");
            tree.AddLevel("Virtual Button");

            Enumerable.Range(1, 16).ToList<Int32>().ForEach(joy =>
            {
                var vJoy = tree.Root.AddNode($"Virtual Joystick #{joy}", "Virtual joystick device to use.");

                Enumerable.Range(1, 128).ToList<Int32>().ForEach(button =>
                {
                    var vButton = vJoy.AddItem($"{joy}|{button}|Latching", $"Virtual Button #{button}", "Button to press on virtual joystick.");
                });
            });

            return tree;
        }

        protected override async void RunCommand(String actionParameter)
        {
            var config = actionParameter.Split("|".ToCharArray());
            var vjoy = UInt32.Parse(config[0]);
            var vbutton = UInt32.Parse(config[1]);
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
            SetButtonState(vjoy, vbutton, state);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter == null || actionParameter.Trim().Length <= 0 || !actionParameter.Contains("|"))
            {
                return this.DisplayName;
            }

            var config = actionParameter.Split("|".ToCharArray());
            var vjoy = UInt32.Parse(config[0]);
            var vbutton = UInt32.Parse(config[1]);
            Boolean state = false;

            if (ButtonStates.ContainsKey(actionParameter))
            {
                // Not sure why this is flipped, but its very consistent
                state = !ButtonStates[actionParameter];
            }
            else
            {
                ButtonStates.Add(actionParameter, false);
            }

            var onOff = state ? "on" : "off";
            return $"Virtual Joystick {vjoy} Button {vbutton} set to {onOff}.";
        }
    }
}
