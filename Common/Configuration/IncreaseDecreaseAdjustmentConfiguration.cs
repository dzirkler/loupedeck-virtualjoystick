namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    using System;

    public class IncreaseDecreaseAdjustmentConfiguration : ConfigurationItemBase
    {
        public IncreaseDecreaseAdjustmentConfiguration() { }

        public UInt32 IncreaseButtonId{ get; set; }
        public UInt32 DecreaseButtonId { get; set; }
        public UInt32 ResetButtonId { get; set; }
    }
}
