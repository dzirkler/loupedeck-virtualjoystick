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

namespace DesertSunSoftware.LoupedeckVirtualJoystick.ConfigGenerator
{
    internal class Program
    {
        /// <summary>
        /// Generates config for virtual joystick plugins.
        /// </summary>
        /// <param name="screen">Only prints to the console, does not save to file</param>
        /// <param name="file"></param>
        static void Main(Boolean screen = false, String file = "")
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
                AnsiConsole.WriteLine(CreateConfig());
                AnsiConsole.Write(whiteRule);
                AnsiConsole.WriteLine();
            }
            else
            {
                File.WriteAllText(file, CreateConfig());

                AnsiConsole.MarkupLine("[bold]Config written to:[/]");
                var path = new TextPath(file)
                    .LeafColor(Color.Aqua);
                AnsiConsole.Write(path);
                AnsiConsole.WriteLine();
            }
        }

        private static String CreateConfig()
        {

            // Initialize Config File
            Debug.WriteLine("Initialize Config File");
            var config = new DrivingSimPluginConfiguration();

            // vJoystick ID
            config.vJoyID = 1;


            // Start Counting Buttons at 1
            UInt32 buttonId = 1;

            // Display Buttons
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Game Info",
                FullName = "Game Started",
                SafeName = CreateSafeName("Game Info", "Game Started"),
                DisplayText = "Game Running:\n {0}",
                ButtonId = 0,
                Style = ButtonConfiguration.ButtonStyle.DisplayButton,
                TelemetryItem = "SdkActive"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Game Info",
                FullName = "Game Paused",
                SafeName = CreateSafeName("Game Info", "Game Paused"),
                DisplayText = "Game Paused:\n {0}",
                ButtonId = 0,
                Style = ButtonConfiguration.ButtonStyle.DisplayButton,
                TelemetryItem = "Paused"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Transmission",
                FullName = "Selected Gear",
                SafeName = CreateSafeName("Engine", "Engine Status"),
                DisplayText = "Auto:\n {0}",
                ButtonId = 0,
                Style = ButtonConfiguration.ButtonStyle.DisplayButton,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.GearDashboards"
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
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Brakes",
                FullName = "Parking Brake",
                SafeName = CreateSafeName("Brakes", "Parking Brake"),
                DisplayText = "Parking Brake\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.MotorValues.BrakeValues.ParkingBrake"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Engine",
                FullName = "Engine Start/Stop",
                SafeName = CreateSafeName("Engine", "Engine Start/Stop"),
                DisplayText = "Engine On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.EngineEnabled"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Cruise Control",
                FullName = "TruckValues.CurrentValues.DashboardValues.CruiseControl",
                SafeName = CreateSafeName("Cruise Control", "Cruise Control On/Off"),
                DisplayText = "Cruise Control On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.CruiseControl"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Drivetrain",
                FullName = "Axle Lift/Lower",
                SafeName = CreateSafeName("Drivetrain", "Axle Lift/Lower"),
                DisplayText = "Axle\nLift On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LiftAxleIndicator"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Drivetrain",
                FullName = "Diff Lock",
                SafeName = CreateSafeName("Drivetrain", "Diff Lock"),
                DisplayText = "Diff Lock On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.DifferentialLock"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "Beacon",
                SafeName = CreateSafeName("Lights", "Beacon"),
                DisplayText = "Beacon On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.Beacon"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "Fog Lights",
                SafeName = CreateSafeName("Lights", "Fog Lights"),
                DisplayText = "Fog Lights On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.AuxFront"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "Hazard Lights",
                SafeName = CreateSafeName("Lights", "Hazard Lights"),
                DisplayText = "Hazard Lights On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.HazardWarningLights"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "High Beams",
                SafeName = CreateSafeName("Lights", "High Beams"),
                DisplayText = "High Beams On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.BeamHigh"
            });
            config.Buttons.Add(new ButtonConfiguration()
            {
                GroupName = "Lights",
                FullName = "Next Light Mode",
                SafeName = CreateSafeName("Lights", "Next Light Mode"),
                DisplayText = "Headlights On:\n {0}",
                ButtonId = buttonId++,
                Style = ButtonConfiguration.ButtonStyle.MomentaryButton,
                TelemetryItem = "TruckValues.CurrentValues.LightsValues.BeamLow"
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
                config.Buttons.Add(new ButtonConfiguration()
                {
                    SafeName = CreateSafeName(button),
                    GroupName = button.Item1,
                    FullName = button.Item2,
                    DisplayText = button.Item3,
                    ButtonId = buttonId++,
                    Style = ButtonConfiguration.ButtonStyle.MomentaryButton
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
                config.Buttons.Add(new ButtonConfiguration()
                {
                    SafeName = CreateSafeName(button),
                    FullName = button.Item2,
                    GroupName = button.Item1,
                    DisplayText = button.Item3,
                    ButtonId = buttonId++,
                    Style = ButtonConfiguration.ButtonStyle.LatchingButton,
                    DefaultValue = false
                });
            }

            // Multi-Position Switches
            //config.MultiPositionSwitches.Add(new MultiPositionSwitchConfiguration()
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
            //config.MultiPositionSwitches.Add(new MultiPositionSwitchConfiguration()
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
            config.IncreaseDecreaseAdjustments.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Cruise Control", "Cruise Control Adjust"),
                GroupName = "Cruise Control",
                FullName = "Cruise Control Adjust",
                DisplayText = "Cruise Control",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.CruiseControl",
            });
            config.IncreaseDecreaseAdjustments.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Transmission", "Drive Mode Adjust"),
                GroupName = "Transmission",
                FullName = "Drive Mode Adjust",
                DisplayText = "Drive Mode",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++
            });
            config.IncreaseDecreaseAdjustments.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Route Advisor", "Route Adviser Pages"),
                GroupName = "Route Advisor",
                FullName = "Route Advisor Pages",
                DisplayText = "Route Advisor",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++
            });
            config.IncreaseDecreaseAdjustments.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Camera", "Camera Change"),
                GroupName = "Camera",
                FullName = "Camera Change",
                DisplayText = "Camera Change",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++
            });
            config.IncreaseDecreaseAdjustments.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Wipers", "Wiper Speed Adjust"),
                GroupName = "Wipers",
                FullName = "Wiper Speed Adjust",
                DisplayText = "Wipers",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++,
                TelemetryItem = "TruckValues.CurrentValues.DashboardValues.Wipers",
            });


            // Full Range Adjustments
            config.FullRangeAdjustments.Add(new FullRangeAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("View", "Left/Right"),
                GroupName = "View",
                FullName = "Left/Right",
                DisplayText = "L/R",
                Axis = FullRangeAdjustmentConfiguration.JoystickAxis.Rx,
                DefaultValue = (config.JoystickAxisMaxValue - config.JoystickAxisMinValue) / 2
            });

            // Save as Init Config
            //Configuration = config;

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var stringResult = serializer.Serialize(config);

            return stringResult;

        }

        private static String CreateSafeName(Tuple<String, String, String, String> button)
        {
            return CreateSafeName(button.Item1, button.Item2);
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
