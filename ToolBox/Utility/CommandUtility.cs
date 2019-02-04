using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ToolBox.Utility.Structs;

namespace ToolBox.Utility
{
    public static class CommandUtility
    {
        public static event EventHandler<Output> CommandOutputReceived;

        public static void Cmd(string commandText)
        {
            Task.Factory.StartNew(() =>
            {
                var process = new Process();
                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C {commandText}",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                process.StartInfo = startInfo;
                process.Start();

                var output = new Output();
                output.Message = "Finished: ";
                output.Message += process.StandardOutput.ReadToEnd();
                output.Message += process.StandardError.ReadToEnd();
                output.ReturnValue = process.ExitCode;
                output.Success = output.ReturnValue == 0;
                
                CommandOutputReceived?.Invoke(null, output);
            });
        }
    }
}
