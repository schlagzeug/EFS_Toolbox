﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ToolBox.Objects.Android;
using ToolBox.Utility;

namespace ToolBox.UserControls
{
    /// <summary>
    /// Interaction logic for ApkInstaller.xaml
    /// </summary>
    public partial class ApkInstaller : UserControl
    {
        // TODO: add support for multiple devices
        public AndroidDevice AndroidDevice = new AndroidDevice("123456789");

        public event EventHandler<string> NotifyStartEvent;

        public ApkInstaller()
        {
            InitializeComponent();
            TextBox_APKDirectory.Text = Settings.ApkDirectory;
        }

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
            InvokeNotifyStartEvent($"Attempting to install {apk}...");
            AndroidDevice.InstallAPK(apk);
        }
        
        protected virtual void InvokeNotifyStartEvent(string s)
        {
            NotifyStartEvent?.Invoke(this, s);
        }
    }
}
