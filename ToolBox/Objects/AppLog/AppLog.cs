using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ToolBox.Objects.AppLog
{
    public class AppLog
    {
        public List<AppLogFile> Files { get; set; }
        
        public AppLog(string directory, string fileNameFilter)
        {
            Files = new List<AppLogFile>();
            foreach (var file in Directory.GetFiles(directory, fileNameFilter))
            {
                var x = new AppLogFile(file);
                Files.Add(x);
            }
        }

        public List<AppLogData> GetAllData()
        {
            var list = new List<AppLogData>();
            foreach (var file in Files)
            {
                list.AddRange(file.Data);
            }

            return list;
        }

        public List<AppLogData> ShowData()
        {
            var sortedData = GetAllData().OrderBy(x => x.Timestamp).ToList();
            return sortedData;
        }
        public List<AppLogData> ShowErrors()
        {
            var result = from x in GetAllData() where x.Type == "ERROR" || x.Type == "WARN" select x;
            var sortedData = result.OrderBy(x => x.Timestamp).ToList();
            return sortedData;
        }
    }
}
