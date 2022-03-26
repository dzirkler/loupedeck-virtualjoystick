﻿using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Formatters
{
    internal class YesNoFormatter : IFormatter
    {
        public string FormatText(IConfigurationItem item, Object telemetry)
        {
            if (telemetry != null && telemetry is Boolean)
            {
                var output = (Boolean)telemetry ? "Yes" : "No";
                return String.Format(item.TextFormatString, output);
            }
            else
            {
                return item.TextFormatString;
            }

        }
    }
}
