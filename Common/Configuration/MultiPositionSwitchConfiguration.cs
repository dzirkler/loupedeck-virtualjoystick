namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    using System;
    using System.Collections.Generic;

    using YamlDotNet.Serialization;

    public class MultiPositionSwitchConfiguration : ConfigurationItemBase
    {
        public MultiPositionSwitchConfiguration() { }

        [YamlIgnore] public Int32 CurrentValue { get; set; }
        public Int32 DefaultValue { get; set; }
        public Int32 Positions { get; set; }
        public List<MultiPositionValue> PositionValues { get; set; }
        public Boolean WrapAround { get; set; }

        public class MultiPositionValue
        {
            public Int32 PositionId { get; set; }
            public UInt32 ButtonId { get; set; }
            public String DisplayName { get; set; }
        }

    }
}
