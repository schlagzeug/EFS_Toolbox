using System;
using System.Windows.Controls;
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

        public void OnEndReceived(object sender, string e)
        {
            if (e != string.Empty) WriteOutput(e);
            WriteBreak();
            WriteOutput(string.Empty);
        }

        protected virtual void OnOutputFinished()
        {
            OutputFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
