using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToolBox.Utility;
using ToolBox.Utility.Structs;

namespace ToolBox.UserControls
{
    /// <summary>
    /// Interaction logic for OutputTextBox.xaml
    /// </summary>
    public partial class OutputTextBox : UserControl
    {
        private readonly string BREAK = new string('-', 30);

        public event EventHandler OutputFinished;

        public OutputTextBox()
        {
            InitializeComponent();
            AndroidTools.BeginOutput += OnStartReceived;
            CommandUtility.CommandOutputReceived += OnOutputReceived;
            AndroidTools.EndOutput += OnEndReceived;
        }

        private void WriteBreak()
        {
            WriteOutput($"{BREAK} {DateTime.Now:yyyy.MM.dd  HH:mm:ss} {BREAK}");
        }

        private void WriteOutput(string output)
        {
            if (TextBox_Output.Dispatcher.CheckAccess())
            {
                TextBox_Output.Text += output + "\r\n";
                TextBox_Output.UpdateLayout();
                TextBox_Output.ScrollToEnd();
            }
            else
            {
                Dispatcher.Invoke((() =>
                {
                    TextBox_Output.Text += output + "\r\n";
                    TextBox_Output.UpdateLayout();
                    TextBox_Output.ScrollToEnd();
                }));
            }
        }

        public void OnStartReceived(object sender, string e)
        {
            WriteBreak();
            WriteOutput(e);
        }

        public void OnOutputReceived(object sender, Output e)
        {
            WriteOutput(e.Message);
            OnOutputFinished();
        }

        public void OnEndReceived(object sender, Output e)
        {
            WriteOutput(e.Message);
            WriteBreak();
            WriteOutput(string.Empty);
        }

        protected virtual void OnOutputFinished()
        {
            OutputFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
