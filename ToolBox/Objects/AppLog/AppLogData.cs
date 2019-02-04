using System;
using System.Globalization;
using System.IO;
using ToolBox.Utility;

namespace ToolBox.Objects.AppLog
{
    public class AppLogData
    {
        public DateTime Timestamp { get; set; }
        public string Thread { get; set; }
        public string Type { get; set; }
        public string Message { get; private set; }
        public string FileName
        {
            get { return Path.GetFileName(FilePath); }
        }
        public int LineNumber { get; set; }

        private string RawString { get; set; }
        private string FilePath { get; set; }

        public AppLogData(string filePath, int lineNum, string data)
        {
            if (data == string.Empty) return;
            RawString = data;

            var provider = CultureInfo.CurrentCulture;
            string[] dateFormats = { "yyyy-MM-dd HH:mm:ss,fff", "yyyyMMdd-HH:mm:ss,fff", "yyyy-MM-dd-HH:mm:ss" };

            FilePath = filePath;
            LineNumber = lineNum;

            try
            {
                // Timestamp
                var dateSubString = data.Substring(0, data.IndexOf('[') - 1);
                DateTime tempDT = DateTime.MinValue;
                DateTime.TryParseExact(dateSubString, dateFormats, provider, DateTimeStyles.AdjustToUniversal, out tempDT);
                Timestamp = tempDT;

                // Thread
                Thread = data.Substring(0, data.IndexOf(']') + 1).Substring(data.IndexOf('['));

                // Type
                Type = data.Substring(data.IndexOf(']') + 1).TrimStart(' ');
                Type = Type.Substring(0, Type.IndexOf(' ')).Trim(' ');

                // Message
                Message = data.Substring(data.IndexOf(Type)).Replace(Type, string.Empty).Trim(' ');
            }
            catch
            {
                Message = data;
            }
        }

        public void AddToMessage(string message)
        {
            Message += "\r\n" + message;
            RawString += "\r\n" + message;
        }

        public override string ToString()
        {
            return RawString;
        }

        public void OpenFile()
        {
            FileSystemUtility.OpenFile(FilePath, false, LineNumber);
        }
    }
}
