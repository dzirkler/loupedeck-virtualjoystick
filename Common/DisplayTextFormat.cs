using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common
{
    public static class DisplayTextFormat
    {
        private static Dictionary<Formatter, IFormatter> _formatters = new Dictionary<Formatter, IFormatter>();

        static DisplayTextFormat()
        {
            _formatters.Add(Formatter.IconOnly, new IconOnlyFormatter());
            _formatters.Add(Formatter.OnOff, new OnOffFormatter());
            _formatters.Add(Formatter.YesNo, new YesNoFormatter());
            _formatters.Add(Formatter.FullName, new FullNameFormatter());
            _formatters.Add(Formatter.FormattedText, new DisplayTextFormatter());
            _formatters.Add(Formatter.Custom, new CustomTextFormatter());
        }

        public static string FormatCommandText(this IConfigurationItem item, Object telemetry)
        {
            return _formatters[item.CommandTextFormatter].FormatText(item, telemetry);
        }

        public static string FormatIconText(this IConfigurationItem item, Object telemetry)
        {
            return _formatters[item.IconTextFormatter].FormatText(item, telemetry);
        }

        public enum Formatter
        {
            IconOnly,
            FullName,
            OnOff,
            YesNo,
            TrueFalse,
            FormattedText,
            Custom
        }

    }
}
