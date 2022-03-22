namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    using System;

    using YamlDotNet.Serialization;

    public class FullRangeAdjustmentConfiguration : ConfigurationItemBase
    {
        public FullRangeAdjustmentConfiguration() { }

        public JoystickAxis Axis { get; set; }
        [YamlIgnore] public Int32 CurrentValue { get; set; }
        public Int32 DefaultValue { get; set; }

        public enum JoystickAxis
        {
            X = 48,
            Y = 49,
            Z = 50,
            Rx = 51,
            Ry = 52,
            Rz = 53,
            Slider0 = 54,
            Slider1 = 55
            //    ,
            //HID_USAGE_WHL = 56,
            //HID_USAGE_POV = 57
        }
    }
}
