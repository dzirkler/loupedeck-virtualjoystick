using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    public class TransmissionGearFormatter : IFormatter
    {
        public string FormatText(IConfigurationItem item, object telemetry)
        {
            if (telemetry == null || !(telemetry is Int32)) return item.FullName;

            int selectedGear = (Int32)telemetry;
            string gear;
            if (selectedGear > 0)
                gear = $"D{selectedGear}";
            else if (selectedGear < 0)
                gear = $"R{Math.Abs(selectedGear)}";
            else
                gear = "N";

            return String.Format(item.TextFormatString, gear);
        }
    }
}
