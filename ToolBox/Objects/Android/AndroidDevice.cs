using System;
using ToolBox.Utility;
using ToolBox.Utility.Constants;
using ToolBox.Utility.Structs;

namespace ToolBox.Objects.Android
{
    public class AndroidDevice
    {
        public string DeviceID { get; private set; }

        public event EventHandler<Output> OutputReceived;

        public AndroidDevice(string deviceID)
        {
            DeviceID = deviceID;
            AndroidUtility.OutputReceived += AndroidUtility_OutputReceived;
        }

        ~AndroidDevice()
        {
            AndroidUtility.OutputReceived -= AndroidUtility_OutputReceived;
        }

        public void InstallAPK(string apkPath)
        {
            AndroidUtility.RunAdbCommand($"install {apkPath}");
        }

        public void UninstallPackage(string packageName)
        {
            AndroidUtility.RunAdbCommand($"uninstall {packageName}");
        }

        public void DeleteDBFolder()
        {
            AndroidUtility.DeleteFolder(FolderPaths.EFS_ANDROID_DB_FOLDER);
        }
        
        public void StopApp(string packageName)
        {
            AndroidUtility.StopApp(packageName);
        }

        public void PullLogs(string destDir)
        {
            AndroidUtility.PullFiles(FolderPaths.EFS_ANDROID_FOLDER, destDir);
        }

        public void PullLogcat(string outputFile)
        {
            AndroidUtility.PullLogcat(outputFile);
        }

        public void ProvisionDevice_Key(string orgID, string provisionKey, string hostName)
        {
            AndroidUtility.AdbKeyEvent(61);
            AndroidUtility.AdbText(orgID);
            AndroidUtility.AdbKeyEvent(61);
            AndroidUtility.AdbText(provisionKey);
            AndroidUtility.AdbKeyEvent(61);
            AndroidUtility.AdbText(hostName);
            AndroidUtility.AdbKeyEvent(61);
            AndroidUtility.AdbKeyEvent(66);
        }

        public void ProvisionDevice_Key(string orgID, string provisionKey, string hostName, string vehicleID)
        {
            AndroidUtility.AdbText(vehicleID);
            ProvisionDevice_Key(orgID, provisionKey, hostName);
        }

        public void ProvisionDevice_Intent(string orgID, string provisionKey, string hostName, string vehicleID)
        {
            AndroidUtility.BroadcastIntent(AndroidIntents.ENSURE_PROVISIONED,
                $"--es vehicleID {vehicleID} --es organizationID {orgID} --es provisioningKey {provisionKey} --es hostName {hostName}");
        }

        public void LogInDriver_Key(string driverID, string password)
        {
            AndroidUtility.AdbText(driverID);
            AndroidUtility.AdbKeyEvent(61);
            AndroidUtility.AdbText(password);
            AndroidUtility.AdbKeyEvent(61);
            AndroidUtility.AdbKeyEvent(66);
        }

        public void LogInDriver_Intent(string driverID, string password)
        {
            AndroidUtility.BroadcastIntent(AndroidIntents.ENSURE_USER_SIGNED_IN, $"--es userID {driverID} --es userPassword {password}");
        }

        private void AndroidUtility_OutputReceived(object sender, Output e)
        {
            OutputReceived?.Invoke(this, e);
        }
    }
}
