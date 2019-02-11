using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Objects.eFleetSuite
{
    public class LoginDriver
    {
        public string DriverID { get; set; }
        public string Password { get; set; }

        public LoginDriver(string driverID, string password)
        {
            DriverID = driverID;
            Password = password;
        }

        public LoginDriver(string compoundString)
        {
            var x = compoundString.Replace("d=", string.Empty).Split('|');
            DriverID = x[0];
            Password = x[1];
        }

        public override string ToString()
        {
            return $"d={DriverID}|{Password}";
        }
    }
}
