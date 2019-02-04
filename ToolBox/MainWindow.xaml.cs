using System.Windows;
using ToolBox.UserControls;

namespace ToolBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppLogTab.Content = new AppLogViewer();
            QuickToolsTab.Content = new QuickTools();
            AndroidToolsTab.Content = new AndroidTools();
        }
    }
}
