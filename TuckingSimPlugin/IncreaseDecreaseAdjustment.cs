namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IncreaseDecreaseAdjustment : IncreaseDecreaseAdjustmentBase
    {
        public IncreaseDecreaseAdjustment() : base(true)
        {

            foreach (var incDecAdjuster in TruckingSimPlugin.Configuration.IncreaseDecreaseAdjustments)
            {
                // Add a Dial
                this.AddParameter(
                    incDecAdjuster.SafeName,
                    incDecAdjuster.FullName,
                    incDecAdjuster.GroupName,
                    "Multi-Position Switch",
                    LoupedeckOperatingSystem.Win);
            }

        }

        protected override async void RunCommand(String actionParameter)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var incDecAdjusters = TruckingSimPlugin.Configuration.IncreaseDecreaseAdjustments;
            var adjuster = incDecAdjusters.Where(b => b.SafeName == actionParameter).First();

            await SendButtonPress(joyId, adjuster.ResetButtonId, TruckingSimPlugin.Configuration.MomentaryButtonDepressTime);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override async void ApplyAdjustment(String actionParameter, Int32 ticks)
        {
            var joyId = TruckingSimPlugin.Configuration.vJoyID;
            var incDecAdjusters = TruckingSimPlugin.Configuration.IncreaseDecreaseAdjustments;
            var adjuster = incDecAdjusters.Where(b => b.SafeName == actionParameter).First();

            // Increase or Decrease?
            var buttonId = ticks > 0 ? adjuster.IncreaseButtonId : adjuster.DecreaseButtonId;

            await SendButtonPress(joyId, buttonId, TruckingSimPlugin.Configuration.MomentaryButtonDepressTime);

            // Update Text/Image
            this.ActionImageChanged(actionParameter);
        }

        protected override String GetCommandDisplayName(String actionParameter, PluginImageSize imageSize)
        {
            var button = TruckingSimPlugin.Configuration.IncreaseDecreaseAdjustments.Find(a => a.SafeName == actionParameter);
            return button == null ? "actionParameter" : button.DisplayText;
        }

    }
}
