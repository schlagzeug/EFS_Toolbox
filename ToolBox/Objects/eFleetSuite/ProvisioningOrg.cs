using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Objects.eFleetSuite
{
    public class ProvisioningOrg
    {
        public string Name { get; set; }
        public string ProvisioningKey { get; set; }
        public List<LoginDriver> Drivers { get; set; }

        public ProvisioningOrg(string name, string key)
        {
            Name = name;
            ProvisioningKey = key;
            Drivers = new List<LoginDriver>();
        }

        public ProvisioningOrg(List<string> list)
        {
            Drivers = new List<LoginDriver>();
            foreach (var element in list)
            {
                if (element.StartsWith("org="))
                {
                    var x = element.Split('|');
                    Name = x[0].Replace("org=", string.Empty);
                    ProvisioningKey = x[1];
                }
                else
                {
                    var d = new LoginDriver(element);
                    Drivers.Add(d);
                }
            }
        }

        public override string ToString()
        {
            var driversString = string.Empty;
            foreach (var loginDriver in Drivers)
            {
                driversString += $" {loginDriver}";
            }

            return $"org={Name}|{ProvisioningKey} {driversString}";
        }
    }
}
