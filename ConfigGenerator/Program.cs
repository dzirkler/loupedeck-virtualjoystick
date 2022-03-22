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

            // Momentary Buttons
            var mommentButtons = new List<Tuple<String, String, String>>
                    {
                        //new Tuple<String, String, String>("Brakes", "Parking Brake", "Parking Brake"),
                        new Tuple<String, String, String>("Camera", "Camera 1", "Camera 1"),
                        new Tuple<String, String, String>("Camera", "Camera 2", "Camera 2"),
                        new Tuple<String, String, String>("Camera", "Camera 3", "Camera 3"),
                        new Tuple<String, String, String>("Camera", "Camera 4", "Camera 4"),
                        new Tuple<String, String, String>("Camera", "Camera 5", "Camera 5"),
                        new Tuple<String, String, String>("Camera", "Camera 6", "Camera 6"),
                        new Tuple<String, String, String>("Camera", "Camera 7", "Camera 7"),
                        new Tuple<String, String, String>("Camera", "Camera 8", "Camera 8"),
                        new Tuple<String, String, String>("Camera", "Next Camera", "Next Camera"),
                        new Tuple<String, String, String>("Drivetrain", "Axle Lift/Lower", "Axle\nLift/\nLower"),
                        new Tuple<String, String, String>("Drivetrain", "Diff Lock", "Diff Lock"),
                        //new Tuple<String, String, String>("Engine", "Engine Start/Stop", "Start/Stop"),
                        new Tuple<String, String, String>("Horns", "Air Horn", "Air Horn"),
                        new Tuple<String, String, String>("Horns", "Horn", "Horn"),
                        new Tuple<String, String, String>("Lights", "Beacon", "Beacon"),
                        new Tuple<String, String, String>("Lights", "Fog Lights", "Fog Lights"),
                        new Tuple<String, String, String>("Lights", "Hazard Lights", "Hazard Lights"),
                        new Tuple<String, String, String>("Lights", "High Beams", "High Beams"),
                        new Tuple<String, String, String>("Lights", "Next Light Mode", "Next Light Mode")
                    };
            foreach (var button in mommentButtons)
            {
                config.Buttons.Add(new ButtonConfiguration()
                {
                    SafeName = CreateSafeName(button),
                    GroupName = button.Item1,
                    FullName = button.Item2,
                    DisplayText = button.Item3,
                    //IconFileName = button.Item2 == "Engine Start/Stop" ? "Icons.engine.png" : null,
                    ButtonId = buttonId++,
                    Style = ButtonConfiguration.ButtonStyle.MomentaryButton
                });
            }

            // Latching Buttons
            var latchingButtons = new List<Tuple<String, String, String>>
                    {
                        new Tuple<String, String, String>("Brakes", "Parking Brake (latching)", "Parking\nBrake\n({0})"),
                        new Tuple<String, String, String>("Engine", "Engine Start/Stop (latching)", "Engine\nStart/Stop\n({0})"),

                    };
            foreach (var button in latchingButtons)
            {
                config.Buttons.Add(new ButtonConfiguration()
                {
                    SafeName = CreateSafeName(button),
                    FullName = button.Item2,
                    GroupName = button.Item1,
                    DisplayText = button.Item3,
                    //IconFileName = button.Item2 == "Engine Start/Stop (latching)" ? "Icons.engine.png" : null,
                    ButtonId = buttonId++,
                    Style= ButtonConfiguration.ButtonStyle.LatchingButton,
                    DefaultValue = false
                });
            }

            // Multi-Position Switches
            config.MultiPositionSwitches.Add(new MultiPositionSwitchConfiguration()
            {
                SafeName = CreateSafeName("Transmission", "Drive Mode"),
                GroupName = "Transmission",
                FullName = "Drive Mode",
                DisplayText = "Auto\n{0}",
                CurrentValue = 2,
                DefaultValue = 2,
                Positions = 3,
                PositionValues = new List<MultiPositionSwitchConfiguration.MultiPositionValue>()
                        {
                            new MultiPositionSwitchConfiguration.MultiPositionValue()
                            {
                                PositionId = 1,
                                ButtonId = buttonId++,
                                DisplayName = "R"
                            },
                            new MultiPositionSwitchConfiguration.MultiPositionValue()
                            {
                                PositionId = 2,
                                ButtonId = UInt32.MaxValue,
                                DisplayName = "N"
                            },
                            new MultiPositionSwitchConfiguration.MultiPositionValue()
                            {
                                PositionId = 3,
                                ButtonId = buttonId++,
                                DisplayName = "D"
                            }
                        },
                WrapAround = false
            });
            config.MultiPositionSwitches.Add(new MultiPositionSwitchConfiguration()
            {
                SafeName = CreateSafeName("Wipers", "Wiper Speed Adjust"),
                GroupName = "Wipers",
                FullName = "Wiper Speed Adjust",
                DisplayText = "Wipers\n{0}",
                CurrentValue = 1,
                DefaultValue = 1,
                Positions = 4,
                PositionValues = new List<MultiPositionSwitchConfiguration.MultiPositionValue>()
                        {
                            new MultiPositionSwitchConfiguration.MultiPositionValue()
                            {
                                PositionId = 1,
                                ButtonId = UInt32.MaxValue,
                                DisplayName = "Off"
                            },
                            new MultiPositionSwitchConfiguration.MultiPositionValue()
                            {
                                PositionId = 2,
                                ButtonId = buttonId++,
                                DisplayName = "Int"
                            },
                            new MultiPositionSwitchConfiguration.MultiPositionValue()
                            {
                                PositionId = 3,
                                ButtonId = buttonId++,
                                DisplayName = "Lo"
                            },
                            new MultiPositionSwitchConfiguration.MultiPositionValue()
                            {
                                PositionId = 4,
                                ButtonId = buttonId++,
                                DisplayName = "Hi"
                            }
                        },
                WrapAround = false
            });

            // Increase/Decrease Adjustments
            config.IncreaseDecreaseAdjustments.Add(new IncreaseDecreaseAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("Cruise Control", "Cruise Control Adjust"),
                GroupName = "Cruise Control",
                FullName = "Cruise Control Adjust",
                DisplayText = "Cruise Control",
                IconFileName = "Icons.car-cruise-control.png",
                IncreaseButtonId = buttonId++,
                DecreaseButtonId = buttonId++,
                ResetButtonId = buttonId++
            });


            // Full Range Adjustments
            config.FullRangeAdjustments.Add(new FullRangeAdjustmentConfiguration()
            {
                SafeName = CreateSafeName("View", "Left/Right"),
                GroupName = "View",
                FullName = "Left/Right",
                DisplayText = "L/R",
                Axis = FullRangeAdjustmentConfiguration.JoystickAxis.X,
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
