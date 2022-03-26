namespace DesertSunSoftware.LoupedeckVirtualJoystick.BeamNgDrivePlugin
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

    public class BeamNgDrivePlugin : Plugin
    {

        public BeamNgDrivePlugin() : base()
        {
            // Load Config 
            LoadConfigurationFromFile();
        }

        public override Boolean HasNoApplication => true;
        public override Boolean UsesApplicationApiOnly => true;
        internal static DrivingSimPluginConfiguration Configuration { get; set; }

        public override void Load()
        {
            this.Info.DisplayName = "BeamNG.drive";

            //this.Info.Icon16x16 = EmbeddedResources.ReadImage("DesertSunSoftware.LoupedeckVirtualJoystick.BeamNgDrivePlugin.Icons.BeamNgDrivePlugin-16.png");
            //this.Info.Icon32x32 = EmbeddedResources.ReadImage("DesertSunSoftware.LoupedeckVirtualJoystick.BeamNgDrivePlugin.Icons.BeamNgDrivePlugin-32.png");
            //this.Info.Icon48x48 = EmbeddedResources.ReadImage("DesertSunSoftware.LoupedeckVirtualJoystick.BeamNgDrivePlugin.Icons.BeamNgDrivePlugin-48.png");
            //this.Info.Icon256x256 = EmbeddedResources.ReadImage("DesertSunSoftware.LoupedeckVirtualJoystick.BeamNgDrivePlugin.Icons.BeamNgDrivePlugin-256.png");

        }

        private void LoadConfigurationFromFile()
        {
            var pluginDataDirectory = this.GetPluginDataDirectory();
            if (IoHelpers.EnsureDirectoryExists(pluginDataDirectory))
            {
                var filePath = Path.Combine(pluginDataDirectory, "BeamNgDriveConfig.yml");

                if (!File.Exists(filePath))
                {
                    // Take it from the default one in the DLL & write it out & continue.
                    File.WriteAllText(filePath, EmbeddedResources.ReadTextFile("DesertSunSoftware.LoupedeckVirtualJoystick.BeamNgDrivePlugin.BeamNgDriveConfig.yml"));
                }

                BeamNgDrivePlugin.Configuration = (DrivingSimPluginConfiguration)File.ReadAllText(filePath).GenerateConfigurationFromString();
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
