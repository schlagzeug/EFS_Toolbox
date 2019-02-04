using System;
using ToolBox.Utility.Structs;

namespace ToolBox.Utility
{
    public static class AndroidUtility
    {
        public static event EventHandler<Output> OutputReceived;

        static AndroidUtility()
        {
            CommandUtility.CommandOutputReceived += CommandUtilityCommandOutputReceived;
        }

        public static void StopApp(string packageName)
        {
            RunAdbCommand($"shell am force-stop {packageName}");
        }

        public static void DeleteFolder(string path)
        {
            RunAdbCommand($"shell rm -r {path}");
        }

        public static void AdbText(string text)
        {
            RunAdbCommand($"shell input text \"{text}\"");
        }

        public static void AdbKeyEvent(int eventID)
        {
            RunAdbCommand($"shell input keyevent {eventID}");
        }

        public static void BroadcastIntent(string intent, string args)
        {
            RunAdbCommand($"shell am start -a \"{intent}\" {args}");
        }

        public static void PullFiles(string source, string dest)
        {
            RunAdbCommand($"pull {source} \"{dest}\"");
        }

        public static void PullLogcat(string outputFile)
        {
            RunAdbCommand($"logcat -d >{outputFile}");
        }

        public static void RunAdbCommand(string input)
        {
            CommandUtility.Cmd($"adb {input}");
        }

        private static void CommandUtilityCommandOutputReceived(object sender, Output e)
        {
            OutputReceived?.Invoke(null, e);
        }
    }
}
