using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    public class ConfigurationItemBase : IConfigurationItem
    {
        public String SafeName { get; set; }
        public String GroupName { get; set; }
        public String FullName { get; set; }
        public String TextFormatString { get; set; }
        public DisplayTextFormat.Formatter CommandTextFormatter { get; set; }
        public DisplayTextFormat.Formatter IconTextFormatter { get; set; }
        public String CustomTextFormatterType { get; set; }
        public String TelemetryItem { get; set; }
    }
}
