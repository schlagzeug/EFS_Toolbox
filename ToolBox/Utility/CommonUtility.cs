using System.Collections.Generic;

namespace ToolBox.Utility
{
    public static class CommonUtility
    {
        public static List<string> GetCommonList(List<string> list1, List<string> list2)
        {
            var retList = new List<string>();
            foreach (var item in list1)
            {
                if (list2.Contains(item))
                {
                    retList.Add(item);
                }
            }
            return retList;
        }
    }
}
