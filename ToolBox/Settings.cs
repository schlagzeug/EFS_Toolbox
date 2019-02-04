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
                return new List<ProvisioningHost>
                {
                    new ProvisioningHost("Local", NetworkUtility.GetCurrentIPAddress()),
                    new ProvisioningHost("Epsilon", "epsilonmobile.iseinc.biz")
                };
            }
            set
            {
                // nothing
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
