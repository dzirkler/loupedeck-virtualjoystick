using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Formatters
{
    internal class IconOnlyFormatter : IFormatter
    {
        public string FormatText(IConfigurationItem item, Object telemetry)
        {
            return String.Empty;
        }
    }
}
