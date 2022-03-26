using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using Spectre.Console;
using DesertSunSoftware.LoupedeckVirtualJoystick.Common;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.ConfigGenerator
{
    internal class Program
    {
        /// <summary>
        /// Generates config for virtual joystick plugins.
        /// </summary>
        /// <param name="screen">Only prints to the console, does not save to file</param>
        /// <param name="file"></param>
        /// <param name="app"></param>
        static void Main(Boolean screen = false, String file = "", String app = "truck")

        {
            Spectre.Console.AnsiConsole.Write(
                new FigletText("ConfigGenerator")
                    .Color(Color.Aqua)
                );

            var whiteRule = new Rule();
            whiteRule.Style = Style.Parse("white");

            if (screen)
            {
                AnsiConsole.Write(whiteRule);
                AnsiConsole.WriteLine();
                if (app == "truck")
                    AnsiConsole.WriteLine(CreateTruckingSimConfig());
                else if (app == "beam")
                    AnsiConsole.WriteLine(CreateBeamNgDriveSimConfig());
                AnsiConsole.Write(whiteRule);
                AnsiConsole.WriteLine();
            }
            else
            {
                if (app == "truck")
                    File.WriteAllText(file, CreateTruckingSimConfig());
                else if (app == "beam")
                    File.WriteAllText(file, CreateBeamNgDriveSimConfig());


                AnsiConsole.MarkupLine("[bold]Config written to:[/]");
                var path = new TextPath(file)
                    .LeafColor(Color.Aqua);
                AnsiConsole.Write(path);
                AnsiConsole.WriteLine();
            }
        }

        private static String CreateBeamNgDriveSimConfig()
        {

            // Initialize Config File
            Debug.WriteLine("Initialize Config File");
            var config = new DrivingSimPluginConfiguration();

            // vJoystick ID
            config.vJoyID = 1;


            // Start Counting Buttons at 1
            UInt32 buttonId = 1;


            // Momentary Buttons
            //config.Buttons.Add(new ButtonConfiguration()
            //{
            //    GroupName = "XXX",
            //    FullName = "XXX",
            //    SafeName = CreateSafeName("XXX", "XXX"),
            //    DisplayText = "XXX On:\n {0}",
            //    ButtonId = buttonId++,
            //    Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
            //    TelemetryItem = "TruckValues.CurrentValues.XXX"
            //});

            var mommentButtons = new List<Tuple<String, String>>
                    {
                        new Tuple<String, String>("Brakes", "Parking Brake"),
                        new Tuple<String, String>("Lights", "Headlights"),
                        new Tuple<String, String>("Lights", "High Beams"),
                        new Tuple<String, String>("Lights", "Hazard Lights"),
                        new Tuple<String, String>("Lights", "Fog Lights"),
                        new Tuple<String, String>("Lights", "Lightbar"),
                        new Tuple<String, String>("Cruise Control", "Cruise Control Disable"),
                        new Tuple<String, String>("Cruise Control", "Cruise Control Set"),
                        new Tuple<String, String>("Cruise Control", "Cruise Control Reset"),
                        new Tuple<String, String>("Cruise Control", "Cruise Control Resume"),
                        new Tuple<String, String>("Drivetrain", "Diff Lock"),
                        new Tuple<String, String>("Drivetrain", "4WD"),
                        new Tuple<String, String>("Engine", "Engine Start/Stop"),
                        new Tuple<String, String>("Horns", "Horn"),
                        new Tuple<String, String>("Misc", "Reset Vehicle"),
                        new Tuple<String, String>("Trailer", "Toggle Couplers"),
                        new Tuple<String, String>("Vehicle", "Explode"),
                    };
            foreach (var button in mommentButtons)
            {
                config.Items.Add(new ButtonConfiguration()
                {
                    SafeName = CreateSafeName(button),
                    GroupName = button.Item1,
                    FullName = button.Item2,
                    ButtonId = buttonId++,
                    Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                    CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                    IconTextFormatter = DisplayTextFormat.Formatter.FullName,
                });
            }

            // Latching Buttons
            var latchingButtons = new List<Tuple<String, String, String>>
            {
                new Tuple<String, String, String>("Lights", "High Beams (Latching)", "High Beams"),
                //new Tuple<String, String, String>("Brakes", "Parking Brake", "Parking\nBrake\n({0})"),
                //new Tuple<String, String, String>("Engine", "Engine Start/Stop", "Engine\nStart/Stop\n({0})"),
            };
            foreach (var button in latchingButtons)
            {
                config.Items.Add(new ButtonConfiguration()
                {
                    SafeName = CreateSafeName(button),
                    FullName = button.Item2,
                    GroupName = button.Item1,
                    TextFormatString = button.Item3,
                    ButtonId = buttonId++,
                    Style = ButtonConfiguration.ButtonStyle.LatchingButton,
                    DefaultValue = false,
                    CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                    IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
                });
            }

            // Multi-Position Switches
            //config.Items.Add(new MultiPositionSwitchConfiguration()
            //{
            //    SafeName = CreateSafeName("Transmission", "Drive Mode"),
            //    GroupName = "Transmission",
            //    FullName = "Drive Mode",
            //    DisplayText = "Auto\n{0}",
            //    DefaultValue = 2,
            //    Positions = 3,
            //    PositionValues = new List<MultiPositionSwitchConfiguration.MultiPositionValue>()
            //            {
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 1,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "R"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 2,
            //                    ButtonId = UInt32.MaxValue,
            //                    DisplayName = "N"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 3,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "D"
            //                }
            //            },
            //    WrapAround = false
            //});
            //config.Items.Add(new MultiPositionSwitchConfiguration()
            //{
            //    SafeName = CreateSafeName("Wipers", "Wiper Speed Adjust"),
            //    GroupName = "Wipers",
            //    FullName = "Wiper Speed Adjust",
            //    DisplayText = "Wipers\n{0}",
            //    DefaultValue = 1,
            //    Positions = 4,
            //    PositionValues = new List<MultiPositionSwitchConfiguration.MultiPositionValue>()
            //            {
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 1,
            //                    ButtonId = UInt32.MaxValue,
            //                    DisplayName = "Off"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 2,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "Int"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 3,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "Lo"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 4,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "Hi"
            //                }
            //            },
            //    WrapAround = false
            //});

            // Increase/Decrease Adjustments
            config.Items.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Cruise Control", "Cruise Control Adjust"),
                GroupName = "Cruise Control",
                FullName = "Cruise Control Adjust",
                TextFormatString = "Cruise Control",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Transmission", "Drive Mode Adjust"),
                GroupName = "Transmission",
                FullName = "Drive Mode Adjust",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.FullName,
            });
            config.Items.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Camera", "Camera Change"),
                GroupName = "Camera",
                FullName = "Camera Change",
                TextFormatString = "Camera Change",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });


            // Full Range Adjustments
            config.Items.Add(new FullRangeAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("View", "Left/Right"),
                GroupName = "View",
                FullName = "Left/Right",
                Axis = FullRangeAdjustmentConfiguration.JoystickAxis.Rx,
                DefaultValue = (config.JoystickAxisMaxValue - config.JoystickAxisMinValue) / 2,
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });

            string configStr = config.GenerateConfigurationText();
            return configStr;

        }

        private static String CreateTruckingSimConfig()
        {

            // Initialize Config File
            Debug.WriteLine("Initialize Config File");
            var config = new DrivingSimPluginConfiguration();

            // vJoystick ID
            config.vJoyID = 1;


            // Start Counting Buttons at 1
            UInt32 buttonId = 1;

            // Display Buttons
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Game Info",
                FullName = "Game Started",
                SafeName = CreateSafeName("Game Info", "Game Started"),
                TextFormatString = "Game Running:\n {0}",
                ButtonId = 0,
                Style = ButtonConfiguration.ButtonStyle.DisplayButton,
                TelemetryItem = "SdkActive",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.YesNo,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Game Info",
                FullName = "Game Paused",
                SafeName = CreateSafeName("Game Info", "Game Paused"),
                TextFormatString = "Game Paused:\n {0}",
                ButtonId = 0,
                Style = ButtonConfiguration.ButtonStyle.DisplayButton,
                TelemetryItem = "Paused",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.YesNo,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Transmission",
                FullName = "Selected Gear",
                SafeName = CreateSafeName("Transmission", "Selected Gear"),
                TextFormatString = "Gear:\n {0}",
                ButtonId = 0,
                Style = ButtonConfiguration.ButtonStyle.DisplayButton,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.GearDashboards",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.Custom,
                CustomTextFormatterType = "DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin.TransmissionGearFormatter, DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin",
            });

            // Momentary Buttons
            //config.Buttons.Add(new ButtonConfiguration()
            //{
            //    GroupName = "XXX",
            //    FullName = "XXX",
            //    SafeName = CreateSafeName("XXX", "XXX"),
            //    DisplayText = "XXX On:\n {0}",
            //    ButtonId = buttonId++,
            //    Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
            //    TelemetryItem = "TruckValues.CurrentValues.XXX"
            //});
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Brakes",
                FullName = "Parking Brake",
                SafeName = CreateSafeName("Brakes", "Parking Brake"),
                TextFormatString = "Parking Brake\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.MotorValues.BrakeValues.ParkingBrake",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Engine",
                FullName = "Engine Start/Stop",
                SafeName = CreateSafeName("Engine", "Engine Start/Stop"),
                TextFormatString = "Engine On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.EngineEnabled",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Cruise Control",
                FullName = "Cruise Control On/Off",
                SafeName = CreateSafeName("Cruise Control", "Cruise Control On/Off"),
                TextFormatString = "Cruise Control On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.CruiseControl",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Drivetrain",
                FullName = "Axle Lift/Lower",
                SafeName = CreateSafeName("Drivetrain", "Axle Lift/Lower"),
                TextFormatString = "Axle\nLift On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LiftAxleIndicator",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Drivetrain",
                FullName = "Diff Lock",
                SafeName = CreateSafeName("Drivetrain", "Diff Lock"),
                TextFormatString = "Diff Lock On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.DifferentialLock",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "Beacon",
                SafeName = CreateSafeName("Lights", "Beacon"),
                TextFormatString = "Beacon On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.Beacon",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "Fog Lights",
                SafeName = CreateSafeName("Lights", "Fog Lights"),
                TextFormatString = "Fog Lights On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.AuxFront",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "Hazard Lights",
                SafeName = CreateSafeName("Lights", "Hazard Lights"),
                TextFormatString = "Hazard Lights On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.HazardWarningLights",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "High Beams",
                SafeName = CreateSafeName("Lights", "High Beams"),
                TextFormatString = "High Beams On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.BeamHigh",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "Next Light Mode",
                SafeName = CreateSafeName("Lights", "Next Light Mode"),
                TextFormatString = "Headlights On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.BeamLow",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });

            var mommentButtons = new List<Tuple<String, String, String>>
                    {
                        new Tuple<String, String, String>("Camera", "Camera 1", "Camera 1"),
                        new Tuple<String, String, String>("Camera", "Camera 2", "Camera 2"),
                        new Tuple<String, String, String>("Camera", "Camera 3", "Camera 3"),
                        new Tuple<String, String, String>("Camera", "Camera 4", "Camera 4"),
                        new Tuple<String, String, String>("Camera", "Camera 5", "Camera 5"),
                        new Tuple<String, String, String>("Camera", "Camera 6", "Camera 6"),
                        new Tuple<String, String, String>("Camera", "Camera 7", "Camera 7"),
                        new Tuple<String, String, String>("Camera", "Camera 8", "Camera 8"),
                        new Tuple<String, String, String>("Camera", "Next Camera", "Next Camera"),
                        new Tuple<String, String, String>("Cruise Control", "Cruise Control Resume", "Cruise Control Resume"),
                        new Tuple<String, String, String>("Horns", "Air Horn", "Air Horn"),
                        new Tuple<String, String, String>("Horns", "Horn", "Horn"),
                        new Tuple<String, String, String>("Misc", "Activate (Enter)", "Activate"),
                        new Tuple<String, String, String>("Trailer", "Trailer Attach/Detatch", "Trailer Attach/Detatch")
                    };
            foreach (var button in mommentButtons)
            {
                config.Items.Add(new ButtonConfiguration()
                {
                    SafeName = CreateSafeName(button),
                    GroupName = button.Item1,
                    FullName = button.Item2,
                    TextFormatString = button.Item3,
                    ButtonId = buttonId++,
                    Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                    CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                    IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
                });
            }

            // Latching Buttons
            var latchingButtons = new List<Tuple<String, String, String>>
            {
                //new Tuple<String, String, String>("Brakes", "Parking Brake", "Parking\nBrake\n({0})"),
                //new Tuple<String, String, String>("Engine", "Engine Start/Stop", "Engine\nStart/Stop\n({0})"),
            };
            foreach (var button in latchingButtons)
            {
                config.Items.Add(new ButtonConfiguration()
                {
                    SafeName = CreateSafeName(button),
                    FullName = button.Item2,
                    GroupName = button.Item1,
                    TextFormatString = button.Item3,
                    ButtonId = buttonId++,
                    Style = ButtonConfiguration.ButtonStyle.LatchingButton,
                    DefaultValue = false,
                    CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                    IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
                });
            }

            // Multi-Position Switches
            //config.Items.Add(new MultiPositionSwitchConfiguration()
            //{
            //    SafeName = CreateSafeName("Transmission", "Drive Mode"),
            //    GroupName = "Transmission",
            //    FullName = "Drive Mode",
            //    DisplayText = "Auto\n{0}",
            //    DefaultValue = 2,
            //    Positions = 3,
            //    PositionValues = new List<MultiPositionSwitchConfiguration.MultiPositionValue>()
            //            {
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 1,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "R"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 2,
            //                    ButtonId = UInt32.MaxValue,
            //                    DisplayName = "N"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 3,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "D"
            //                }
            //            },
            //    WrapAround = false
            //});
            //config.Items.Add(new MultiPositionSwitchConfiguration()
            //{
            //    SafeName = CreateSafeName("Wipers", "Wiper Speed Adjust"),
            //    GroupName = "Wipers",
            //    FullName = "Wiper Speed Adjust",
            //    DisplayText = "Wipers\n{0}",
            //    DefaultValue = 1,
            //    Positions = 4,
            //    PositionValues = new List<MultiPositionSwitchConfiguration.MultiPositionValue>()
            //            {
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 1,
            //                    ButtonId = UInt32.MaxValue,
            //                    DisplayName = "Off"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 2,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "Int"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 3,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "Lo"
            //                },
            //                new MultiPositionSwitchConfiguration.MultiPositionValue()
            //                {
            //                    PositionId = 4,
            //                    ButtonId = buttonId++,
            //                    DisplayName = "Hi"
            //                }
            //            },
            //    WrapAround = false
            //});

            // Increase/Decrease Adjustments
            config.Items.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Cruise Control", "Cruise Control Adjust"),
                GroupName = "Cruise Control",
                FullName = "Cruise Control Adjust",
                TextFormatString = "Cruise Control",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.CruiseControl",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Transmission", "Drive Mode Adjust"),
                GroupName = "Transmission",
                FullName = "Drive Mode Adjust",
                TextFormatString = "Gear:\n {0}",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.GearDashboards",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.Custom,
                CustomTextFormatterType = "DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin.TransmissionGearFormatter, DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin",
            });
            config.Items.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Route Advisor", "Route Adviser Pages"),
                GroupName = "Route Advisor",
                FullName = "Route Advisor Pages",
                TextFormatString = "Route Advisor",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Camera", "Camera Change"),
                GroupName = "Camera",
                FullName = "Camera Change",
                TextFormatString = "Camera Change",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });
            config.Items.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Wipers", "Wiper Speed Adjust"),
                GroupName = "Wipers",
                FullName = "Wiper Speed Adjust",
                TextFormatString = "Wipers",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.Wipers",
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });


            // Full Range Adjustments
            config.Items.Add(new FullRangeAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("View", "Left/Right"),
                GroupName = "View",
                FullName = "Left/Right",
                TextFormatString = "L/R",
                Axis = FullRangeAdjustmentConfiguration.JoystickAxis.Rx,
                DefaultValue = (config.JoystickAxisMaxValue - config.JoystickAxisMinValue) / 2,
                CommandTextFormatter = DisplayTextFormat.Formatter.FullName,
                IconTextFormatter = DisplayTextFormat.Formatter.IconOnly,
            });

            string configStr = config.GenerateConfigurationText();
            return configStr;

        }

        private static String CreateSafeName(Tuple<String, String, String> button)
        {
            return CreateSafeName(button.Item1, button.Item2);
        }

        private static String CreateSafeName(Tuple<String, String> button)
        {
            return CreateSafeName(button.Item1, button.Item2);
        }

        private static String CreateSafeName(String groupName, String displayName)
        {
            return String.Concat(
                String.Concat(groupName.Where(Char.IsLetter)),
                "-",
                String.Concat(displayName.Where(Char.IsLetterOrDigit))
                );
        }

    }
}
