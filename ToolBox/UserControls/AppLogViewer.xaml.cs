using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToolBox.Objects.AppLog;
using ToolBox.Utility;

namespace ToolBox.UserControls
{
    /// <summary>
    /// Interaction logic for AppLogViewer.xaml
    /// </summary>
    public partial class AppLogViewer : UserControl
    {
        public AppLogViewer()
        {
            InitializeComponent();
        }
        
        private void Button_ChooseFolder_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPath = FileSystemUtility.OpenFolderDialog();
            if (selectedPath != string.Empty) TextBox_Folder.Text = selectedPath;
        }

        private void Button_ViewLogs_OnClick(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(TextBox_Folder.Text))
            {
                DataGrid.ItemsSource = new AppLog(TextBox_Folder.Text, TextBox_Filter.Text).GetAllData();
            }
        }

        private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var x = DataGrid.SelectedItem as AppLogData;
            x?.OpenFile();
        }

        private void Button_OutputSelectedLog_OnClick(object sender, RoutedEventArgs e)
        {
            var x = new AppLogFile(string.Empty);
            var file = Path.Combine(TextBox_Folder.Text, "output.txt");
            foreach (AppLogData item in DataGrid.SelectedItems)
            {
                x.Add(item);
            }
            
            x.WriteToFile(file);
            FileSystemUtility.OpenFile(file);
        }
    }
}
