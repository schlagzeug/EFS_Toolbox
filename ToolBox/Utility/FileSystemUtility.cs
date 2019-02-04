using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ToolBox.Utility.Structs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace ToolBox.Utility
{
    public static class FileSystemUtility
    {
        public static bool? OpenFileDialog(string directory, string filter, string defaultExt, string fileName, out string returnedFileName)
        {
            var openDiag = new OpenFileDialog
            {
                InitialDirectory = directory,
                Multiselect = false,
                Filter = filter,
                DefaultExt = defaultExt,
                FileName = fileName
            };

            var result = openDiag.ShowDialog();
            returnedFileName = openDiag.FileName;
            return result;
        }

        public static string OpenFolderDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.ShowDialog();
                return dialog.SelectedPath;
            }
        }

        public static void OpenFile(string path)
        {
            OpenFile(path, false, -1);
        }

        public static void OpenFile(string path, bool openAsReadOnly, int lineNumber)
        {
            if (File.Exists(path))
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo()
                {
                    UseShellExecute = false,
                    FileName = @"C:\Program Files\Notepad++\notepad++.exe",
                    Arguments = $"{(openAsReadOnly ? "-ro" : string.Empty)} \"{path}\" {(lineNumber > 0 ? "-n" + lineNumber : string.Empty)}"
                };
                p.Start();
            }
            else
            {
                NotificationUtility.ShowError("File doesn't exist.");
            }
        }

        /// <summary>Removes a folder and subfolders and retuns a list containing the return value and a message</summary>
        public static Output RemoveFolder(string folderPath)
        {
            var output = new Output();
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
                if (Directory.Exists(folderPath))
                {
                    output.Success = false;
                    output.Message = "Issue deleting folder.";
                }
                else
                {
                    output.Success = true;
                    output.Message = "Folder Deleted.";
                }
            }
            else
            {
                output.Success = false;
                output.Message = "Directory doesn't exist!";
            }
            return output;
        }

        /// <summary>Opens a directory in Windows Explorer</summary>
        public static void OpenDirectory(string path)
        {
            if (path == string.Empty)
            {
                NotificationUtility.ShowError("Directory doesn't exist.");
                return;
            }
            DirectoryInfo diDir = new DirectoryInfo(path);
            if (diDir.Exists)
            {
                Process.Start(path);
            }
            else
            {
                NotificationUtility.ShowError($"Directory ({path}) doesn't exist.");
            }
        }
    }
}
