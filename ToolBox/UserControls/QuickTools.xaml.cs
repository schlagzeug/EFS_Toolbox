using System.IO;
using System.Windows;
using System.Windows.Controls;
using ToolBox.Utility;
using ToolBox.Utility.Constants;

namespace ToolBox.UserControls
{
    /// <summary>
    /// Interaction logic for QuickTools.xaml
    /// </summary>
    public partial class QuickTools : UserControl
    {
        public QuickTools()
        {
            InitializeComponent();
            PopulateIPAddress();
        }

        private void Button_DeleteVSFolderForMobile_OnClick(object sender, RoutedEventArgs e)
        {
            var output = FileSystemUtility.RemoveFolder(Path.Combine(FolderPaths.EFS_ANDROID_REPO, @"eFleetDroid\.vs"));
            NotificationUtility.ShowMessage(output.Message);
        }

        private void Button_IpAddressRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            PopulateIPAddress();
        }

        private void PopulateIPAddress()
        {
            TextBox_IpAddresses.Text = NetworkUtility.GetCurrentIPAddress();
        }
    }
}
