﻿namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    using System;

    public class ButtonConfiguration : ConfigurationItemBase
    {
        public ButtonConfiguration() { }

        public UInt32 ButtonId { get; set; }
        public ButtonStyle Style { get; set; }
        public Boolean DefaultValue { get; set; }
        
        public enum ButtonStyle
        {
            MomentaryButton,
            LatchingButton,
            DisplayButton
        }
    }
}
