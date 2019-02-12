using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ToolBox.Objects.Android;
using ToolBox.Objects.eFleetSuite;
using ToolBox.Utility;
using ToolBox.Utility.Constants;

namespace ToolBox.UserControls
{
    /// <summary>
    /// Interaction logic for AndroidCommands.xaml
    /// </summary>
    public partial class AndroidCommands : UserControl
    {
        // TODO: add support for multiple devices
        public AndroidDevice AndroidDevice = new AndroidDevice("123456789");

        public List<ProvisioningHost> HostList { get; set; }

        public event EventHandler<string> NotifyStartEvent;

        public AndroidCommands()
        {
            InitializeComponent();
            HostList = Settings.HostList;
            foreach (var provisioningHost in HostList)
            {
                ComboBox_Host.Items.Add(provisioningHost.DisplayName);
            }

            TextBox_VehicleID.Text = "ELDSim";
            CheckBox_UseIntent.IsChecked = true;
            UpdateUI();
        }

        private void Button_StopEFS_OnClick(object sender, RoutedEventArgs e)
        {
            StopApp(AndroidPackage.EFLEETSUITE);
        }

        private void Button_StopELDSim_OnClick(object sender, RoutedEventArgs e)
        {
            StopApp(AndroidPackage.ELDSIM);
        }

        private void Button_StopWatchdog_OnClick(object sender, RoutedEventArgs e)
        {
            StopApp(AndroidPackage.WATCHDOG);
        }

        private void StopApp(string packageName)
        {
            InvokeNotifyStartEvent($"Stopping {packageName}...");
            AndroidDevice.StopApp(packageName);
        }

        private void Button_UninstallEFS_OnClick(object sender, RoutedEventArgs e)
        {
            UninstallPackage(AndroidPackage.EFLEETSUITE);
        }

        private void Button_UninstallELDSim_OnClick(object sender, RoutedEventArgs e)
        {
            UninstallPackage(AndroidPackage.ELDSIM);
        }

        private void Button_UninstallWatchdog_OnClick(object sender, RoutedEventArgs e)
        {
            UninstallPackage(AndroidPackage.WATCHDOG);
        }

        private void UninstallPackage(string packageName)
        {
            InvokeNotifyStartEvent($"Attempting to Uninstall {packageName}");
            AndroidDevice.UninstallPackage(packageName);
        }

        private void Button_DeleteDBFolder_OnClick(object sender, RoutedEventArgs e)
        {
            InvokeNotifyStartEvent("Deleting eFleetSuite DB folder...");
            AndroidDevice.DeleteDBFolder();
        }

        private void Button_PullEFSLogs_OnClick(object sender, RoutedEventArgs e)
        {
            InvokeNotifyStartEvent("Pulling logs...");
            AndroidDevice.PullLogs(Settings.DeviceApplogSaveDirctory);
            FileSystemUtility.OpenDirectory(Path.Combine(Settings.DeviceApplogSaveDirctory, "eFleetDroid"));
        }

        private void Button_PullLogcat_OnClick(object sender, RoutedEventArgs e)
        {
            InvokeNotifyStartEvent("Pulling Logcat...");
            var outputFile = Path.Combine(Settings.LogCatSaveDirectory, "logcat.txt");
            AndroidDevice.PullLogcat(outputFile);
            FileSystemUtility.OpenFile(outputFile);
        }

        private void ComboBox_Host_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Org.Items.Clear();

            var host = GetSelectedHost();
            if (host != null)
            {
                foreach (var provisioningOrg in host.OrgList)
                {
                    ComboBox_Org.Items.Add(provisioningOrg.Name);
                }
            }

            UpdateUI();
        }

        private void ComboBox_Org_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox_Driver.Items.Clear();

            var org = GetSelectedOrg();
            if (org != null)
            {
                foreach (var driver in org.Drivers)
                {
                    ComboBox_Driver.Items.Add(driver.DriverID);
                }
            }

            UpdateUI();
        }

        private void Button_Provision_OnClick(object sender, RoutedEventArgs e)
        {
            var host = GetSelectedHost();
            var org = GetSelectedOrg();
            if (host == null)
            {
                NotificationUtility.ShowError("Invalid Host");
                return;
            }

            var vehicle = TextBox_VehicleID.Text;

            if (CheckBox_UseIntent.IsChecked == true)
            {
                InvokeNotifyStartEvent($"Provisioning device to Org \"{org.Name}\" with intent...");
                AndroidDevice.ProvisionDevice_Intent(org.Name, org.ProvisioningKey, host.HostName, vehicle);
            }
            else
            {
                InvokeNotifyStartEvent($"Provisioning device to Org \"{org.Name}\" with keystrokes...");
                AndroidDevice.ProvisionDevice_Key(org.Name, org.ProvisioningKey, host.HostName, vehicle);
            }
        }

        private void ComboBox_Driver_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI();
        }

        private void Button_DriverLogin_OnClick(object sender, RoutedEventArgs e)
        {
            var driver = GetSelectedDriver();
            if (driver == null) return;

            if (CheckBox_UseIntent.IsChecked == true)
            {
                InvokeNotifyStartEvent($"Logging in Driver \"{driver.DriverID}\" with intent...");
                AndroidDevice.LogInDriver_Intent(driver.DriverID, driver.Password);
            }
            else
            {
                InvokeNotifyStartEvent($"Logging in Driver \"{driver.DriverID}\" with keystrokes...");
                AndroidDevice.LogInDriver_Key(driver.DriverID, driver.Password);
            }
        }

        private ProvisioningHost GetSelectedHost()
        {
            var hostDisplayName = ComboBox_Host.SelectedValue?.ToString();

            if (hostDisplayName == null) return null;

            foreach (var provisioningHost in HostList)
            {
                if (provisioningHost.DisplayName == hostDisplayName.ToString())
                {
                    return provisioningHost;
                }
            }

            return null;
        }

        private ProvisioningOrg GetSelectedOrg()
        {
            var host = GetSelectedHost();
            var selectedOrg = ComboBox_Org.SelectedValue?.ToString();

            if (host == null || selectedOrg == null) return null;
            
            foreach (var provisioningOrg in host.OrgList)
            {
                if (provisioningOrg.Name == selectedOrg)
                {
                    return provisioningOrg;
                }
            }

            return null;
        }

        private LoginDriver GetSelectedDriver()
        {
            var org = GetSelectedOrg();
            var selectedDriver = ComboBox_Driver.SelectedValue?.ToString();

            if (org == null || selectedDriver == null) return null;

            foreach (var driver in org.Drivers)
            {
                if (driver.DriverID == selectedDriver)
                {
                    return driver;
                }
            }

            return null;
        }

        private void UpdateUI()
        {
            var host = GetSelectedHost();
            var org = GetSelectedOrg();
            var driver = GetSelectedDriver();

            ComboBox_Org.IsEnabled = host != null;
            ComboBox_Driver.IsEnabled = (host != null && org != null);
            Button_Provision.IsEnabled = (host != null && org != null);
            Button_DriverLogin.IsEnabled = driver != null;
        }

        protected virtual void InvokeNotifyStartEvent(string s)
        {
            NotifyStartEvent?.Invoke(this, s);
        }
    }
}
