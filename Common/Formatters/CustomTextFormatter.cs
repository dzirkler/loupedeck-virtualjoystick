using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Formatters
{
    internal class CustomTextFormatter : IFormatter
    {
        private static Dictionary<String, IFormatter> _cachedFormatters = new Dictionary<String, IFormatter>();

        public string FormatText(IConfigurationItem item, Object telemetry)
        {
            try
            {
                if (!_cachedFormatters.ContainsKey(item.CustomTextFormatterType))
                {
                    _cachedFormatters.Add(item.CustomTextFormatterType, (IFormatter)Activator.CreateInstance(Type.GetType(item.CustomTextFormatterType)));
                }

                return _cachedFormatters[item.CustomTextFormatterType].FormatText(item, telemetry);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return item.FullName;
            }
        }
    }
}
