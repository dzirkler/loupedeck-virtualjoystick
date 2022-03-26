using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    public interface IConfigurationItem
    {
        String SafeName { get; set; }
        String GroupName { get; set; }
        String FullName { get; set; }
        String TextFormatString { get; set; }
        DisplayTextFormat.Formatter CommandTextFormatter { get; set; }
        DisplayTextFormat.Formatter IconTextFormatter { get; set; }
        String CustomTextFormatterType {  get; set; }
        String TelemetryItem { get; set; }
    }
}
