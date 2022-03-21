namespace DesertSunSoftware.LoupedeckVirtualJoystick.GenericJoystickPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;

    public class GenericMomentaryButtonCommand : MomentaryButtonCommandBase
    {
        public GenericMomentaryButtonCommand() : base()
        {
            this.MakeProfileAction("tree");
            this.DisplayName = "Momentary Button";
            this.Description = "Simulates pushing a joystick button and then releasing after a short period.";
        }

        protected override PluginProfileActionData GetProfileActionData()
        {
            var tree = new PluginProfileActionTree("Choose Virtual Joystick and Virtual Button");
            tree.AddLevel("Virtual Joystick");
            tree.AddLevel("Virtual Button");

            Enumerable.Range(1, 16).ToList<Int32>().ForEach(joy =>
            {
                var vJoy = tree.Root.AddNode($"Virtual Joystick #{joy}", "Virtual joystick device to use.");

                Enumerable.Range(1, 128).ToList<Int32>().ForEach(button =>
                {
                    var vButton = vJoy.AddItem($"{joy}|{button}|Momentary", $"Virtual Button #{button}", "Button to press on virtual joystick.");
                });
            });

            return tree;
        }

        protected override async void RunCommand(String actionParameter)
        {
            var config = actionParameter.Split("|".ToCharArray());
            var vjoy = UInt32.Parse(config[0]);
            var vbutton = UInt32.Parse(config[1]);

            await SendButtonPress(vjoy, vbutton, 100);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);

        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            if (actionParameter == null || actionParameter.Trim().Length <= 0 || !actionParameter.Contains("|"))
            {
                return this.DisplayName;
                //return this.GetParameter(actionParameter).DisplayName;
                //return null;
            }

            var config = actionParameter.Split("|".ToCharArray());
            var vjoy = UInt32.Parse(config[0]);
            var vbutton = UInt32.Parse(config[1]);

            return $"Virtual Joystick {vjoy} Button {vbutton} pushed.";
        }
    }
}
