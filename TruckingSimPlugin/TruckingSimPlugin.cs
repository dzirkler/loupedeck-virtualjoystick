namespace DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common;
    using DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration;
    using Loupedeck;
    using Pather.CSharp;
    using SCSSdkClient;
    using SCSSdkClient.Object;

    public class TruckingSimPlugin : Plugin
    {
        private SCSSdkTelemetry _rawTelemetry = new SCSSdkTelemetry();
        public static IObservable<SCSTelemetry> RawTelemetry;
        public static IObservable<TelemetryItem> Telemetry;

        public TruckingSimPlugin() : base()
        {
            // Load Config 
            LoadConfigurationFromFile();

            var resolver = new Resolver();
            var telemetryItems = Configuration
                        .Items
                        .Where(i => i.TelemetryItem != null && i.TelemetryItem != String.Empty)
                        .Select(i => i.TelemetryItem);

            RawTelemetry = Observable
                            .FromEvent<SCSSdkClient.TelemetryData, SCSTelemetry>(
                                onNextHandler => (SCSTelemetry data, bool newTimestamp) => onNextHandler(data),
                                h => _rawTelemetry.Data += h,
                                h => _rawTelemetry.Data -= h
                            );
            Telemetry = RawTelemetry
                            .SelectMany(i =>
                            {
                                return telemetryItems.ToObservable().Select(item =>
                                    new TelemetryItem() { Item = item, Value = resolver.ResolveSafe(i, item) }
                                );
                            })
                            .DistinctUntilChanged();

        }

        public override Boolean HasNoApplication => true;
        public override Boolean UsesApplicationApiOnly => true;
        internal static DrivingSimPluginConfiguration Configuration { get; set; }

        public override void Load()
        {
            this.Info.DisplayName = "ATS/ETS2 Simulators";

            this.Info.Icon16x16 = EmbeddedResources.ReadImage("DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin.Icons.TruckingSimPlugin-16.png");
            this.Info.Icon32x32 = EmbeddedResources.ReadImage("DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin.Icons.TruckingSimPlugin-32.png");
            this.Info.Icon48x48 = EmbeddedResources.ReadImage("DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin.Icons.TruckingSimPlugin-48.png");
            this.Info.Icon256x256 = EmbeddedResources.ReadImage("DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin.Icons.TruckingSimPlugin-256.png");

        }

        private void LoadConfigurationFromFile()
        {
            var pluginDataDirectory = this.GetPluginDataDirectory();
            if (IoHelpers.EnsureDirectoryExists(pluginDataDirectory))
            {
                var filePath = Path.Combine(pluginDataDirectory, "TruckingSimConfig.yml");

                if (!File.Exists(filePath))
                {
                    // Take it from the default one in the DLL & write it out & continue.
                    File.WriteAllText(filePath, EmbeddedResources.ReadTextFile("DesertSunSoftware.LoupedeckVirtualJoystick.TruckingSimPlugin.TruckingSimConfig.yml"));
                }

                TruckingSimPlugin.Configuration = (DrivingSimPluginConfiguration)File.ReadAllText(filePath).GenerateConfigurationFromString();
            }
        }

        public override void Unload()
        {
        }

        private void OnApplicationStarted(Object sender, EventArgs e)
        {
        }

        private void OnApplicationStopped(Object sender, EventArgs e)
        {
        }

        public override void RunCommand(String commandName, String parameter)
        {
        }

        public override void ApplyAdjustment(String adjustmentName, String parameter, Int32 diff)
        {
        }

    }
}
