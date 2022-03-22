namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    using System;

    using YamlDotNet.Serialization;

    public class LatchingButtonConfiguration : ConfigurationItemBase
    {
        public LatchingButtonConfiguration() { }

        public UInt32 ButtonId { get; set; }
        public Boolean DefaultValue { get; set; }
        [YamlIgnore] public Boolean CurrentValue { get; set; }
    }
}
