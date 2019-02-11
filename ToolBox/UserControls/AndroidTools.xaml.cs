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
        public static event EventHandler<string> BeginOutput;
        public static event EventHandler<string> EndOutput;

        private EFleetSuiteActions EFleetSuiteActions;
        private AndroidCommands AndroidCommands;
        private ApkInstaller ApkInstaller;
        private OutputTextBox OutputTextBox;

        // TODO: add support for multiple devices
        public AndroidDevice AndroidDevice = new AndroidDevice("123456789");
        
        public AndroidTools()
        {
            InitializeComponent();

            EFleetSuiteActions = new EFleetSuiteActions();
            EFSTabUserControl.Content = EFleetSuiteActions;
            EFleetSuiteActions.NotifyStartEvent += HandleStartEventFromChild;

            AndroidCommands = new AndroidCommands();
            CommandsTabUserControl.Content = AndroidCommands;
            AndroidCommands.NotifyStartEvent += HandleStartEventFromChild;

            ApkInstaller = new ApkInstaller();
            ApkTabControl.Content = ApkInstaller;
            ApkInstaller.NotifyStartEvent += HandleStartEventFromChild;

            OutputTextBox = new OutputTextBox();
            OutputTextBoxControl.Content = OutputTextBox;
            OutputTextBox.OutputFinished += OutputFinished;
        }

        private void HandleStartEventFromChild(object sender, string s)
        {
            EnableTabControl(false);
            BeginOutput?.Invoke(this, s);
        }

        private void EnableTabControl(bool isEnabled)
        {
            EnableElement(TabControl, isEnabled);
        }

        private void EnableElement(UIElement element, bool isEnabled)
        {
            if (element.Dispatcher.CheckAccess())
            {
                element.IsEnabled = isEnabled;
            }
            else
            {
                Dispatcher.Invoke((() =>
                {
                    element.IsEnabled = isEnabled;
                }));
            }
        }

        private void OutputFinished(object sender, EventArgs args)
        {
            EnableTabControl(true);
            OnEndOutput(string.Empty);
        }

        protected virtual void OnEndOutput(string e)
        {
            EndOutput?.Invoke(this, e);
        }
    }
}
