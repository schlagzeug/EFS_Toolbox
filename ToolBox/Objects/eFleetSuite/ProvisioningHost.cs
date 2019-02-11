using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace ToolBox.Objects.eFleetSuite
{
    public class ProvisioningHost
    {
        public string DisplayName { get; set; }
        public string HostName { get; set; }
        public string ConnectionString { get; set; }
        public List<ProvisioningOrg> OrgList { get; set; }

        public ProvisioningHost(string displayName, string hostName)
        {
            DisplayName = displayName;
            HostName = hostName;
            OrgList = new List<ProvisioningOrg>();
        }

        public ProvisioningHost(string hostString)
        {
            var elements = hostString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var names = elements[0].Replace("host=", string.Empty).Split('|');
            DisplayName = names[0];
            HostName = names[1];
            OrgList = new List<ProvisioningOrg>();

            var orgList = new List<string>();
            for (int i = 1; i < elements.Length; i++)
            {
                if (elements[i].StartsWith("org="))
                {
                    if (orgList.Count > 0)
                    {
                        var p = new ProvisioningOrg(orgList);
                        OrgList.Add(p);
                        orgList.Clear();
                    }
                }

                orgList.Add(elements[i]);
            }
            if (orgList.Count > 0)
            {
                var p = new ProvisioningOrg(orgList);
                OrgList.Add(p);
            }
        }

        public override string ToString()
        {
            var orgsString = string.Empty;
            foreach (var provisioningOrg in OrgList)
            {
                orgsString += $" {provisioningOrg}";
            }

            return $"host={DisplayName}|{HostName} {orgsString}";
        }
    }
}
