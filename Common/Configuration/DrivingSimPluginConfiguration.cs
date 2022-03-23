namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DrivingSimPluginConfiguration
    {
        public DrivingSimPluginConfiguration()
        {
            this.Buttons = new List<ButtonConfiguration>();
            this.MultiPositionSwitches = new List<MultiPositionSwitchConfiguration>();
            this.IncreaseDecreaseAdjustments = new List<IncreaseDecreaseAdjustmentConfiguration>();
            this.FullRangeAdjustments = new List<FullRangeAdjustmentConfiguration>();
            this.JoystickAxisMinValue = 0;
            this.JoystickAxisMaxValue = 32767;
            this.JoystickAxisIncrementValue = 100;
            this.MomentaryButtonDepressTime = 100;
        }

        public UInt16 vJoyID { get; set; }

        public Int32 JoystickAxisMinValue { get; set; }
        public Int32 JoystickAxisMaxValue { get; set; }
        public Int32 JoystickAxisIncrementValue { get; set; }
        public Int32 MomentaryButtonDepressTime { get; set; }
        public List<ButtonConfiguration> Buttons { get; set; }
        public List<MultiPositionSwitchConfiguration> MultiPositionSwitches { get; set; }
        public List<IncreaseDecreaseAdjustmentConfiguration> IncreaseDecreaseAdjustments { get; set; }
        public List<FullRangeAdjustmentConfiguration> FullRangeAdjustments { get; set; }

    }

}
