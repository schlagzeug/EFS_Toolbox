using System.Collections.Generic;
using ToolBox.Objects.eFleetSuite;
using ToolBox.Utility;

namespace ToolBox
{
    public static class Settings
    {
        public static List<ProvisioningHost> HostList
        {
            get
            {
                var hostsString = UserSettings.Default.ProvisioningHosts;
                var x = hostsString.Split('~');
                var hostList = new List<ProvisioningHost>();

                foreach (var item in x)
                {
                    var host = new ProvisioningHost(item);
                    hostList.Add(host);
                }

                return hostList;
            }
            set
            {
                var hostsString = string.Empty;
                foreach (var provisioningHost in value)
                {
                    hostsString += provisioningHost.ToString();
                    hostsString += "~";
                }

                hostsString.TrimEnd('~');
                UserSettings.Default.ProvisioningHosts = hostsString;
            }
        }

        public static string ApkDirectory
        {
            get { return UserSettings.Default.ApkDirectory; }
            set { UserSettings.Default.ApkDirectory = value; }
        }

        public static string LogCatSaveDirectory
        {
            get { return UserSettings.Default.LogCatSaveDirectory; }
            set { UserSettings.Default.LogCatSaveDirectory = value; }
        }

        public static string DeviceApplogSaveDirctory
        {
            get { return UserSettings.Default.DeviceAppLogSaveDirectory; }
            set { UserSettings.Default.DeviceAppLogSaveDirectory = value; }
        }
    }
}
