using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    interface IConfigurationItem
    {
        String SafeName { get; set; }
        String GroupName { get; set; }
        String FullName { get; set; }
        String DisplayText { get; set; }
        String IconFileName { get; set; }
    }
}
