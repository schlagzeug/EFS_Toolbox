using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolBox.Objects.AppLog
{
    public class AppLogFile : IList<AppLogData>
    {
        public IList<AppLogData> Data { get; }
        public AppLogFile(string filePath)
        {
            Data = new List<AppLogData>();
            if (!File.Exists(filePath)) return;

            var lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Count(); i++)
            {
                var x = new AppLogData(filePath, i + 1, lines[i]);
                while (i + 1 < lines.Count() && !Regex.IsMatch(lines[i + 1], @"^\d"))
                {
                    i++;
                    x.AddToMessage(lines[i]);
                }

                Data.Add(x);
            }
        }
        
        public IEnumerator<AppLogData> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public override string ToString()
        {
            var output = new StringBuilder();

            foreach (var data in Data)
            {
                output.Append(data);
            }

            return output.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(AppLogData item)
        {
            Data.Add(item);
        }

        public void Clear()
        {
            Data.Clear();
        }

        public bool Contains(AppLogData item)
        {
            return Data.Contains(item);
        }

        public void CopyTo(AppLogData[] array, int arrayIndex)
        {
            Data.CopyTo(array, arrayIndex);
        }

        public bool Remove(AppLogData item)
        {
            return Data.Remove(item);
        }

        public int Count { get; }
        public bool IsReadOnly { get; }
        public int IndexOf(AppLogData item)
        {
            return Data.IndexOf(item);
        }

        public void Insert(int index, AppLogData item)
        {
            Data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Data.RemoveAt(index);
        }

        public AppLogData this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public void WriteToFile(string filePath)
        {
            var sortedData = Data.OrderBy(x => x.Timestamp).ToList();
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var appLogData in sortedData)
                {
                    writer.WriteLine(appLogData.ToString());
                }
            }
        }
    }
}
