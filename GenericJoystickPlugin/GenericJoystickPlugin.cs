namespace DesertSunSoftware.LoupedeckVirtualJoystick.GenericJoystickPlugin
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using Loupedeck;

    public class GenericJoystickPlugin : Plugin
    {
        public GenericJoystickPlugin() : base()
        {
        }

        public override Boolean HasNoApplication => true;
        public override Boolean UsesApplicationApiOnly => true;

        public override void Load()
        {
            this.Info.DisplayName = "Virtual Joystick";
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
