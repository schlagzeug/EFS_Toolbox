using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ToolBox.Objects.Android;
using ToolBox.Objects.eFleetSuite;
using ToolBox.Utility;
using ToolBox.Utility.Constants;
using ToolBox.Utility.Structs;

namespace ToolBox.UserControls
{
    /// <summary>
    /// Interaction logic for AndroidTools.xaml
    /// </summary>
    public partial class AndroidTools : UserControl
    {
        private readonly string BREAK = new string('-', 30);

        public static event EventHandler<string> BeginOutput;
        public static event EventHandler<Output> EndOutput;

        private ApkInstaller ApkInstaller;
        private OutputTextBox OutputTextBox;

        // TODO: add support for multiple devices
        public AndroidDevice AndroidDevice = new AndroidDevice("123456789");
        
        public AndroidTools()
        {
            InitializeComponent();
            //AndroidDevice.OutputReceived += AndroidDevice_OutputReceived;
            TextBox_APKDirectory.Text = Settings.ApkDirectory;
            foreach (var host in Settings.HostList)
            {
                ComboBox_Host.Items.Add(host.Name);
            }
            OutputTextBox = new OutputTextBox();
            ApkInstaller = new ApkInstaller();
            
            OutputTextBoxControl.Content = OutputTextBox;
            OutputTextBox.OutputFinished += OutputFinished;

            ApkInstaller.InstallAPK += ApkInstallerOnInstallApk;
        }

        private void ApkInstallerOnInstallApk(object sender, string e)
        {
            BeginOutput?.Invoke(this, e);
        }


        #region eFleetSuite Tab Handlers

        private void ComboBox_Host_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var host = ComboBox_Host.SelectedItem as ProvisioningHost;
            if (host == null)
            {
                NotificationUtility.ShowError("Invalid Host");
                ComboBox_Host.SelectedIndex = -1;
            }
        }

        private void Button_Provision_OnClick(object sender, RoutedEventArgs e)
        {
            var host = ComboBox_Host.SelectedValue as ProvisioningHost;
            if (host == null)
            {
                NotificationUtility.ShowError("Invalid Host");
                return;
            }

            var key = "";
            var org = ComboBox_Org.SelectedValue.ToString();
            var vehicle = TextBox_VehicleID.Text;

            if (CheckBox_UseIntent_Provision.IsChecked == true)
            {
                AndroidDevice.ProvisionDevice_Intent(org, key, host.ConnectionString, vehicle);
            }
            else
            {
                AndroidDevice.ProvisionDevice_Key(org, key, host.ConnectionString, vehicle);
            }
        }

        private void Button_DriverLogin_OnClick(object sender, RoutedEventArgs e)
        {
            var driverID = ComboBox_Driver.SelectedValue.ToString();
            var password = TextBox_Password.Text == string.Empty ? "aaaa" : TextBox_Password.Text;

            if (CheckBox_UseIntent_DriverLogin.IsChecked == true)
            {
                AndroidDevice.LogInDriver_Intent(driverID, password);
            }
            else
            {
                AndroidDevice.LogInDriver_Key(driverID, password);
            }
        }

        #endregion

        #region Commands Tab Handlers

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
            StartAction($"Stopping {packageName}...");
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
            StartAction($"Attempting to Uninstall {packageName}");
            AndroidDevice.UninstallPackage(packageName);
        }

        private void Button_DeleteDBFolder_OnClick(object sender, RoutedEventArgs e)
        {
            StartAction("Deleting eFleetSuite DB folder...");
            AndroidDevice.DeleteDBFolder();
        }

        private void Button_PullEFSLogs_OnClick(object sender, RoutedEventArgs e)
        {
            StartAction("Pulling logs...");
            AndroidDevice.PullLogs(Settings.DeviceApplogSaveDirctory);
            FileSystemUtility.OpenDirectory(Path.Combine(Settings.DeviceApplogSaveDirctory, "eFleetDroid"));
        }

        private void Button_PullLogcat_OnClick(object sender, RoutedEventArgs e)
        {
            StartAction("Pulling Logcat...");
            var outputFile = Path.Combine(Settings.LogCatSaveDirectory, "logcat.txt");
            AndroidDevice.PullLogcat(outputFile);
            FileSystemUtility.OpenFile(outputFile);
        }
        #endregion

        #region APK Tab Handlers

        private void TextBox_APKDirectory_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ListBox_APKList.ItemsSource = null;
            var tempDirValue = TextBox_APKDirectory.Text.Trim();
            if (Directory.Exists(tempDirValue))
            {
                var filePaths = Directory.GetFiles(tempDirValue, "*.apk");
                var files = new List<string>();
                foreach (var filePath in filePaths)
                {
                    files.Add(Path.GetFileName(filePath));
                }
                files.Sort();
                ListBox_APKList.ItemsSource = files;
                Settings.ApkDirectory = tempDirValue;
            }
        }

        private void Button_ChooseFolder_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPath = FileSystemUtility.OpenFolderDialog();
            if (selectedPath != string.Empty) TextBox_APKDirectory.Text = selectedPath;
        }

        private void Button_InstallAPK_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBox_APKList.SelectedItem == null)
            {
                NotificationUtility.ShowMessage("Please select an APK.");
                return;
            }

            var apk = Path.Combine(TextBox_APKDirectory.Text, ListBox_APKList.SelectedItem.ToString());
            StartAction($"Attempting to install {apk}...");
            AndroidDevice.InstallAPK(apk);
        }

        #endregion


        private void ActivateGrids(bool isEnabled)
        {
            ActivateGrid(Grid_APK, isEnabled);
            ActivateGrid(Grid_Buttons, isEnabled);
            ActivateGrid(Grid_eFS, isEnabled);
        }

        //private void WriteBreak()
        //{
        //    WriteOutput($"{BREAK} {DateTime.Now:yyyy.MM.dd  HH:mm:ss} {BREAK}");
        //}
        
        private void StartAction(string message)
        {
            OnBeginOutput(message);
            //WriteBreak();
            //WriteOutput(message);
            ActivateGrids(false);
        }

        private void ActivateGrid(Grid grid, bool isEnabled)
        {
            if (grid.Dispatcher.CheckAccess())
            {
                grid.IsEnabled = isEnabled;
            }
            else
            {
                Dispatcher.Invoke((() =>
                {
                    grid.IsEnabled = isEnabled;
                }));
            }
        }

        //private void WriteOutput(string output)
        //{
        //    if (TextBox_Output.Dispatcher.CheckAccess())
        //    {
        //        TextBox_Output.Text += output + "\r\n";
        //        TextBox_Output.UpdateLayout();
        //        TextBox_Output.ScrollToEnd();
        //    }
        //    else
        //    {
        //        Dispatcher.Invoke((() =>
        //        {
        //            TextBox_Output.Text += output + "\r\n";
        //            TextBox_Output.UpdateLayout();
        //            TextBox_Output.ScrollToEnd();
        //        }));
        //    }
        //}

        //private void AndroidDevice_OutputReceived(object sender, Output e)
        //{
        //    WriteOutput(e.Message);
        //    WriteBreak();
        //    WriteOutput(string.Empty);
        //    ActivateGrids(true);
        //}

        private void OutputFinished(object sender, EventArgs args)
        {
            ActivateGrids(true);
            OnEndOutput(new Output());
        }

        //protected virtual void OnBeginOutput(string e)
        //{
        //    BeginOutput?.Invoke(this, e);
        //}

        protected virtual void OnEndOutput(Output e)
        {
            EndOutput?.Invoke(this, e);
        }

        private static void OnBeginOutput(string e)
        {
            BeginOutput?.Invoke(null, e);
        }
    }
}
