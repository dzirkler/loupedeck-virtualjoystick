namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using Loupedeck;
    using Pather.CSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IncreaseDecreaseAdjustment : IncreaseDecreaseAdjustmentBase
    {
        private Dictionary<String, Object> Telemetry = new Dictionary<String, Object>();

        public IncreaseDecreaseAdjustment() : base(true)
        {
            var resolver = new Resolver();

            foreach (var adjuster in TruckingSimPlugin.Configuration.IncreaseDecreaseAdjustments)
            {
                // Add a Dial
                this.AddParameter(
                    adjuster.SafeName,
                    adjuster.FullName,
                    adjuster.GroupName,
                    "Increase Decrease Adjustments",
                    LoupedeckOperatingSystem.Win);


                // Seed Storage
                Telemetry.Add(adjuster.SafeName, false);

                TruckingSimPlugin.Telemetry
                    .Select(data => {
                        return resolver.ResolveSafe(data, adjuster.TelemetryItem);
                    })
                    .DistinctUntilChanged()
                    .Subscribe(itemValue => {
                        this.Telemetry[adjuster.SafeName] = itemValue;
                        this.ActionImageChanged(adjuster.SafeName);
                    });

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
            if (actionParameter == null || actionParameter == "") return null;

            var adjuster = TruckingSimPlugin.Configuration.IncreaseDecreaseAdjustments.Find(b => b.SafeName == actionParameter);
            return adjuster == null ? "actionParameter" : String.Format(adjuster.DisplayText, Telemetry[actionParameter]);
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            //return actionParameter.GetIconImage(GetCommandDisplayName(actionParameter, imageSize));
            var adjuster = TruckingSimPlugin.Configuration.IncreaseDecreaseAdjustments.Where(a => a.SafeName == actionParameter).First();
            if (adjuster.TelemetryItem != null)
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
