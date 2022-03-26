using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DesertSunSoftware.LoupedeckVirtualJoystick.Common.Configuration
{
    public static class ConfigurationExtensions
    {

        public static string GenerateConfigurationText(this Object configurationObject)
        {

            var serializer = new SerializerBuilder()
                .DisableAliases()
                .EnsureRoundtrip()
                .WithTagMapping("!DrivingSim", typeof(DrivingSimPluginConfiguration))
                .WithTagMapping("!Button", typeof(ButtonConfiguration))
                .WithTagMapping("!IncreaseDecreaseAdjustment", typeof(IncreaseDecreaseAdjustmentConfiguration))
                .WithTagMapping("!FullRangeAdjustment", typeof(FullRangeAdjustmentConfiguration))
                .WithTagMapping("!MultiPositionSwitch", typeof(MultiPositionSwitchConfiguration))
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var stringResult = serializer.Serialize(configurationObject);

            return stringResult;
        }

        public static Object GenerateConfigurationFromString(this String configurationString)
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .WithTagMapping("!DrivingSim", typeof(DrivingSimPluginConfiguration))
                .WithTagMapping("!Button", typeof(ButtonConfiguration))
                .WithTagMapping("!IncreaseDecreaseAdjustment", typeof(IncreaseDecreaseAdjustmentConfiguration))
                .WithTagMapping("!FullRangeAdjustment", typeof(FullRangeAdjustmentConfiguration))
                .WithTagMapping("!MultiPositionSwitch", typeof(MultiPositionSwitchConfiguration))
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var config = deserializer.Deserialize<DrivingSimPluginConfiguration>(configurationString);
            return config;
        }

    }
}
