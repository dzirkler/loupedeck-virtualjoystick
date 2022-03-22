namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MultiPositionSwitch : MultiPositionSwitchBase
    {
        private Dictionary<String, Int32> SwitchStates = new Dictionary<String, Int32>();

        public MultiPositionSwitch() : base(true)
        {

            foreach (var mpSwitch in TruckingSimPlugin.Configuration.MultiPositionSwitches)
            {
                // Add a Dial
                this.AddParameter(
                    mpSwitch.SafeName,
                    mpSwitch.FullName,
                    mpSwitch.GroupName,
                    "Multi-Position Switch",
                    LoupedeckOperatingSystem.Win);

                SwitchStates.Add(mpSwitch.SafeName, mpSwitch.DefaultValue);
            }

        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var mpSwitches = TruckingSimPlugin.Configuration.MultiPositionSwitches;
            var mpSwitch = mpSwitches.Where(d => d.SafeName == actionParameter).First();
            
            // Reset to default value on press
            SwitchStates[actionParameter] = mpSwitch.DefaultValue;

            //Set vJoy State
            foreach (var position in mpSwitch.PositionValues)
            {
                if (position.ButtonId != UInt32.MaxValue)
                    SetButtonState(joyId, position.ButtonId, SwitchStates[actionParameter] == position.PositionId);
            }

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var mpSwitches = TruckingSimPlugin.Configuration.MultiPositionSwitches;
            var mpSwitch = mpSwitches.Where(d => d.SafeName == actionParameter).First();

            // Increment
            var newValue = SwitchStates[actionParameter];
            newValue += ticks;
            if (newValue > mpSwitch.Positions)
                newValue = mpSwitch.WrapAround ? 1 : mpSwitch.Positions;
            if (newValue < 1)
                newValue = mpSwitch.WrapAround ? mpSwitch.Positions : 1;
            SwitchStates[actionParameter] = newValue;

            //Set vJoy State
            foreach (var position in mpSwitch.PositionValues)
            {
                if (position.ButtonId != UInt32.MaxValue)
                    SetButtonState(joyId, position.ButtonId, SwitchStates[actionParameter] == position.PositionId);
            }

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {

            if (actionParameter == null)
                return String.Empty;

            var mpSwitches = TruckingSimPlugin.Configuration.MultiPositionSwitches;
            var mpSwitch = mpSwitches.Where(mps => mps.SafeName == actionParameter).First();
            if (mpSwitch == null)
            {
                return "actionParameter";
            }

            var currVal = mpSwitch.PositionValues.Find(mps => mps.PositionId == SwitchStates[actionParameter]).DisplayName;
            return String.Format(mpSwitch.DisplayText, currVal);
        }

    }
}
