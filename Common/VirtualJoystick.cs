namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using vJoyInterfaceWrap;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;

    public static class VirtualJoystick
    {
        private static Dictionary<UInt32, vJoy> _joysticks = new Dictionary<UInt32, vJoy>();

        private static vJoy GetJoystick(UInt32 joystickId)
        {
            if (!_joysticks.ContainsKey(joystickId))
            {
                if (!Init(joystickId))
                {
                    throw new InvalidOperationException("Cannot connect to vJoy. Please confirm it is running and restart Loupedeck software.");
                }
            }

            return _joysticks[joystickId];
        }


        private static Boolean Init(UInt32 joystickId)
        {
            // Create one joystick object and a position structure.
            var joystick = new vJoy();

            // Device ID can only be in the range 1-16
            //           if (args.Length > 0 && !String.IsNullOrEmpty(args[0]))
            //                id = Convert.ToUInt32(args[0]);
            if (joystickId <= 0 || joystickId > 16)
            {
                Debug.WriteLine("Illegal device ID {0}\nExit!", joystickId);
                return false;
            }

            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
            {
                Debug.WriteLine("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                return false;
            }
            else
            {
                Debug.WriteLine("Vendor: {0}\nProduct :{1}\nVersion Number:{2}\n", joystick.GetvJoyManufacturerString(), joystick.GetvJoyProductString(), joystick.GetvJoySerialNumberString());
            }

            // Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus(joystickId);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    Debug.WriteLine("vJoy Device {0} is already owned by this feeder\n", joystickId);
                    break;
                case VjdStat.VJD_STAT_FREE:
                    Debug.WriteLine("vJoy Device {0} is free\n", joystickId);
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    Debug.WriteLine("vJoy Device {0} is already owned by another feeder\nCannot continue\n", joystickId);
                    return false;
                case VjdStat.VJD_STAT_MISS:
                    Debug.WriteLine("vJoy Device {0} is not installed or disabled\nCannot continue\n", joystickId);
                    return false;
                default:
                    Debug.WriteLine("vJoy Device {0} general error\nCannot continue\n", joystickId);
                    return false;
            };

            // Check which axes are supported
            var AxisX = joystick.GetVJDAxisExist(joystickId, HID_USAGES.HID_USAGE_X);
            var AxisY = joystick.GetVJDAxisExist(joystickId, HID_USAGES.HID_USAGE_Y);
            var AxisZ = joystick.GetVJDAxisExist(joystickId, HID_USAGES.HID_USAGE_Z);
            var AxisRX = joystick.GetVJDAxisExist(joystickId, HID_USAGES.HID_USAGE_RX);
            var AxisRY = joystick.GetVJDAxisExist(joystickId, HID_USAGES.HID_USAGE_RY);
            var AxisRZ = joystick.GetVJDAxisExist(joystickId, HID_USAGES.HID_USAGE_RZ);
            var AxisSL0 = joystick.GetVJDAxisExist(joystickId, HID_USAGES.HID_USAGE_SL0);
            var AxisSL1 = joystick.GetVJDAxisExist(joystickId, HID_USAGES.HID_USAGE_SL1);
            // Get the number of buttons and POV Hat switchessupported by this vJoy device
            var nButtons = joystick.GetVJDButtonNumber(joystickId);
            var ContPovNumber = joystick.GetVJDContPovNumber(joystickId);
            var DiscPovNumber = joystick.GetVJDDiscPovNumber(joystickId);

            // Print results
            Debug.WriteLine("\nvJoy Device {0} capabilities:\n", joystickId);
            Debug.WriteLine("Numner of buttons\t\t{0}\n", nButtons);
            Debug.WriteLine("Numner of Continuous POVs\t{0}\n", ContPovNumber);
            Debug.WriteLine("Numner of Descrete POVs\t\t{0}\n", DiscPovNumber);
            Debug.WriteLine("Axis X\t\t{0}\n", AxisX ? "Yes" : "No");
            Debug.WriteLine("Axis Y\t\t{0}\n", AxisY ? "Yes" : "No");
            Debug.WriteLine("Axis Z\t\t{0}\n", AxisZ ? "Yes" : "No");
            Debug.WriteLine("Axis Rx\t\t{0}\n", AxisRX ? "Yes" : "No");
            Debug.WriteLine("Axis Ry\t\t{0}\n", AxisRY ? "Yes" : "No");
            Debug.WriteLine("Axis Rz\t\t{0}\n", AxisRZ ? "Yes" : "No");
            Debug.WriteLine("Axis SL0\t\t{0}\n", AxisSL0 ? "Yes" : "No");
            Debug.WriteLine("Axis SL1\t\t{0}\n", AxisSL1 ? "Yes" : "No");

            // Test if DLL matches the driver
            UInt32 DllVer = 0, DrvVer = 0;
            var match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (match)
            {
                Debug.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", DllVer);
            }
            else
            {
                Debug.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);
            }

            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(joystickId))))
            {
                Debug.WriteLine("Failed to acquire vJoy device number {0}.\n", joystickId);
                return false;
            }
            else
            {
                // Reset to initial values
                joystick.ResetVJD(joystickId);

                Debug.WriteLine("Acquired: vJoy device number {0}.\n", joystickId);
                _joysticks.Add(joystickId, joystick);
                return true;
            }

        }

        public static Boolean SendButtonPress(UInt32 joyId, UInt32 buttonId, Boolean state)
        {
            return GetJoystick(joyId).SetBtn(state, joyId, buttonId);
        }

        public static Boolean SetAxis(UInt32 joyId, FullRangeAdjustmentConfiguration.JoystickAxis joystickAxis, Int32 currentValue)
        {
            return GetJoystick(joyId).SetAxis(currentValue, joyId, (HID_USAGES)joystickAxis);
        }

    }
}
